// Copyright 2020, G.Christopher Warrington<c45207@mygcw.net>
//
// This software is released under the GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3. A copy of this license is included in the file LICENSE.
//
// SPDX-License-Identifier: AGPL-3.0-only

namespace Example.GroupsIo
{
    using System;

    public class Error
    {
        public string Object { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Extra { get; set; } = string.Empty;
    }

    public class User
    {
        public UInt64 Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string CsrfToken { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public bool MondayStart { get; set; }

        // etc.
    }

    public class LoginResponse
    {
        public User User { get; set; } = new User();

        public string? Token { get; set; }
    }
}
