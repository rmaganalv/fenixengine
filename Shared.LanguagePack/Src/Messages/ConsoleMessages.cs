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
using System.Globalization;
using System.Resources;
using Shared.LanguagePack.Src.Messages.Contract;

namespace Shared.LanguagePack.Src.Messages;

/// <summary>
/// Implements the IMessageService interface to provide message retrieval functionality from resource files.
/// </summary>
public class ProcessMessages : IMessageBoxes
{
    private readonly ResourceManager _resourceManager =
    new ResourceManager("Shared.LanguagePack.Src.Messages.ProcessMessages", typeof(ProcessMessages).Assembly);

    ///Users/rmagana/Documents/Local/Proyects/MCP/AppCore.Generator/Src/Utils/Messages/MessageService.cs
    /// <summary>
    /// Retrieves a message string from the resource files based on the provided key and the current UI culture.
    /// </summary>
    /// <param name="key"></param>
    /// <rexturns></returns>
    private string GetMessage(string key)
    {
        return _resourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? $"[{key}]";
    }

    /// <summary>
    /// Retrieves the Start process message string from the resource files based on the current UI culture.
    /// </summary>
    /// <returns></returns>
    public string GetStart()
    {
        return GetMessage("Error");
    }

    /// <summary>
    /// Retrieves the error message string from the resource files based on the current UI culture.
    /// </summary>
    /// <returns></returns>
    public string GetErrorMessage()
    {
        return GetMessage("Error");
    }

    /// <summary>
    /// Retrieves a validation message string from the resource files based on the provided key and the current UI culture.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetProcessMessage(string key)
    {
        return GetMessage(key);
    }

    public string GetStart(string key)
    {
        throw new NotImplementedException();
    }

    public string GetErrorMessage(string key)
    {
        throw new NotImplementedException();
    }
}
