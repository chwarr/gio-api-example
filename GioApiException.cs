// Copyright 2020, G.Christopher Warrington<c45207@mygcw.net>
//
// This software is released under the GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3. A copy of this license is included in the file LICENSE.
//
// SPDX-License-Identifier: AGPL-3.0-only

namespace Example.GroupsIo
{
    using System;

    public class GioApiException : Exception
    {
        public GioApiException(
            string message,
            Error? error = null,
            Exception? inner = null)
            : base(message, inner)
        {
            Error = error;
        }

        public Error? Error { get; }
    }
}
