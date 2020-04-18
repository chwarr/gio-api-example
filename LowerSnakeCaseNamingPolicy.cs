// Copyright 2020, G.Christopher Warrington<c45207@mygcw.net>
//
// This software is released under the GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3. A copy of this license is included in the file LICENSE.
//
// SPDX-License-Identifier: AGPL-3.0-only

namespace Example.GroupsIo
{
    using System;
    using System.Text;
    using System.Text.Json;

    public class LowerSnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            var result = new StringBuilder(2 * name.Length);

            bool first = true;

            foreach (char c in name)
            {
                if (Char.IsUpper(c))
                {
                    if (!first)
                    {
                        result.Append('_');
                    }

                    result.Append(Char.ToLowerInvariant(c));
                }
                else
                {
                    result.Append(c);
                }

                first = false;
            }

            return result.ToString();
        }
    }
}
