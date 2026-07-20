// Copyright (C) 2026 Ruben Magaña Alvarado
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
using System.Collections;
using System.Globalization;
using System.Resources;
using Shared.LanguagePack.Src.Messages.Contract;

namespace Shared.LanguagePack.Src.Messages;

/// <summary>
/// Implements the IMessageService interface to provide message retrieval functionality from resource files.
/// </summary>
public class DirectoryMessages : IMessageBoxes
{
    private readonly ResourceManager _resourceManager =
    new ResourceManager("Shared.LanguagePack.Src.Messages.DirectoryMessages", typeof(DirectoryMessages).Assembly);

    
    public string GetErrorMessage(string key)
    {

        switch(key)
        {
            case "101":
            break; 
        }

       return GetProcessMessage("");
    }

    public string GetProcessMessage(string key)
    {
        return _resourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? $"[{key}]";
    }

    public string GetStart(string key)
    {
        return GetProcessMessage( "");
    }
}