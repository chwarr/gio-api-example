// Copyright 2020, G.Christopher Warrington<c45207@mygcw.net>
//
// This software is released under the GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3. A copy of this license is included in the file LICENSE.
//
// SPDX-License-Identifier: AGPL-3.0-only

namespace Example.GroupsIo
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.WebUtilities;

    using static System.FormattableString;


    class Program
    {
        private const string GroupsIoUriBase = "https://groups.io";

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new LowerSnakeCaseNamingPolicy(),
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = true,
        };

        private static readonly HttpClient _client = new HttpClient();

        static async Task Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
                    Usage();
                    Environment.Exit(1);
                }

                string email = args[0];
                string newLocation = args[1];

                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                Console.Write("Enter 2FA code. Blank for none: ");
                string twoFactorCode = Console.ReadLine();

                LoginResponse loginResponse = await Login(email, password, twoFactorCode);

                Console.WriteLine("Logged in. Response:");
                Console.WriteLine(JsonSerializer.Serialize(loginResponse, _jsonOptions));

                User updatedUser = await SetUserLocation(loginResponse.User, newLocation);

                Console.WriteLine("Updated locations. Response:");
                Console.WriteLine(JsonSerializer.Serialize(updatedUser, _jsonOptions));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");

                if (ex is GioApiException apiException)
                {
                    Console.Error.WriteLine($"    - type: {apiException.Error?.Type}");
                    Console.Error.WriteLine($"    - extra: {apiException.Error?.Extra}");
                }

                Console.Error.WriteLine();
                Console.Error.WriteLine(ex.StackTrace);

                Environment.Exit(2);
            }
        }

        private static void Usage()
        {
            Console.WriteLine(@"<email> <new-location>

Updates the provided Groups.io user profile with the given location.
");
        }

        private static async Task<LoginResponse> Login(
            string email,
            string password,
            string? twoFactorCode)
        {
            string loginEndpoint = Invariant($"{GroupsIoUriBase}/api/v1/login");

            var parameters = new Dictionary<string, string>(3)
            {
                ["email"] = email,
                ["password"] = password,
            };

            if (!string.IsNullOrEmpty(twoFactorCode))
            {
                parameters.Add("twofactor", twoFactorCode);
            }

            string loginUri = QueryHelpers.AddQueryString(loginEndpoint, parameters);

            HttpResponseMessage response = await _client.PostAsync(loginUri, content: null);

            if (!response.IsSuccessStatusCode)
            {
                Error? error = await JsonSerializer.DeserializeAsync<Error>(
                    await response.Content.ReadAsStreamAsync(),
                    _jsonOptions);
                throw new GioApiException("Login failed", error);
            }

            return await JsonSerializer.DeserializeAsync<LoginResponse>(
                await response.Content.ReadAsStreamAsync(),
                _jsonOptions);
        }

        private static async Task<User> SetUserLocation(User user, string newLocation)
        {
            string updateEndpoint = Invariant($"{GroupsIoUriBase}/api/v1/updateprofile");

            var parameters = new Dictionary<string, string>(2)
            {
                ["csrf"] = user.CsrfToken,
                ["location"] = newLocation
            };

            string updateUri = QueryHelpers.AddQueryString(updateEndpoint, parameters);

            HttpResponseMessage response = await _client.PostAsync(updateUri, content: null);

            if (!response.IsSuccessStatusCode)
            {
                Error? error = await JsonSerializer.DeserializeAsync<Error>(
                    await response.Content.ReadAsStreamAsync(),
                    _jsonOptions);
                throw new GioApiException("Login failed", error);
            }

            return await JsonSerializer.DeserializeAsync<User>(
                await response.Content.ReadAsStreamAsync(),
                _jsonOptions);
        }
    }
}
