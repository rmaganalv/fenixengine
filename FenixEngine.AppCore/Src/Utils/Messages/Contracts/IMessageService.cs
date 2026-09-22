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

namespace FenixEngine.AppCore.Utils.Messages;

public interface IMessageService
{  
    /// <summary>
    /// Retrieves a message string from the resource files based on the provided key and the current UI culture.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string GetMessage(string key);
    /// <summary>
    /// Retrieves the error message string from the resource files based on the current UI culture.
    /// </summary>
    /// <returns></returns>
    string GetErrorMessage();
    /// <summary>
    /// Retrieves a validation message string from the resource files based on the provided key and the current UI culture.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string GetValidateMessage(string key);
}