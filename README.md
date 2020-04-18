# Groups.io API example in C\#

This example shows how to call two [Groups.io][gio] [APIs][api] from a C#
app:

* [`login`][login]
* [`updateprofile`][updateprofile]

I've chosen to use a .NET Core 3.1 app.

## Running

Ensure you have a copy of [.NET Core SDK 3.1][dotnet] or later installed.
(.NET 5 or later will work too.)

Clone this repository:

```cmd
git clone http://github.com/chwarr/gio-api-example.git
```

Run the example by giving it the email to login as and a new location for
that user's profile:

```cmd
dotnet run foo@example.com "Somewhere on the Internet"
```

You will be prompted for the password and a 2FA code.

## Credentials

The Groups.io API has two ways of performing authentication: you can either
use a login token from the login response or use cookies. This example uses
cookies as it is easier to program against, as `HttpClient` silently handles
parsing them from responses and adding them to requests, and it enables
access to more Groups.io features.

## License

Copyright 2020, G. Christopher Warrington

This software is released under the GNU AFFERO GENERAL PUBLIC LICENSE
Version 3. A copy of this license is included in the file LICENSE.

[api]: https://groups.io/api
[dotnet]: https://dotnet.microsoft.com/download
[gio]: https://groups.io
[login]: https://groups.io/api#login
[updateprofile]: https://groups.io/api#update_profile
