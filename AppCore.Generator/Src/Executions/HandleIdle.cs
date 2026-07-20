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
using AppCore.Generator.src.Utils.Messages;
using AppCore.Generator.Utils;
using AppCore.Generator.Utils.Messages;

namespace AppCore.Generator.Executions;

/// <summary>
/// 
/// </summary>
public static class HandleIdleExecution
{
    private static IMessageService messageService = new MessagesService(); 
    // 📄 Leer especificación
    public static async Task<StatesGenerator> HandleIdle()
    {
        HandleExtension.HandleStart(messageService.GetMessage("Reading File"));

        var spec = await File.ReadAllTextAsync("spec.md");//("../../../spec.md");
        ///Users/rmagana/Documents/Local/Proyects/MCP/SandBox/bin/Debug/net10.0/spec.md
        // Parsear items desde el .md
        if (string.IsNullOrWhiteSpace(spec))
        {
            return HandleExtension.HandleError(messageService.GetErrorMessage());
        }
        
        var items = spec.Split('\n')
                    .Where(line => line.StartsWith("-"))
                    .Select(line => line.TrimStart('-', ' '))
                    .ToArray();

        return HandleExtension.HandleFinish(messageService.GetMessage("Finish Reading"));
    }

 
}