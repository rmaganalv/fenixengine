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
public static class HandleValidated
{
    private static IMessageService messageService = new MessagesService(); 
    
    private static string featureFolder = Path.Combine("Features", featureName ?? "");
    private static string featureName = "Calculos"; 
    

    // 📄 🔍 Validando carpetas y recursos.
    public static async Task<StatesGenerator> HandleValidating()
    {
        HandleExtension.HandleStart(messageService.GetMessage("Start Validate"));

        if (!Directory.Exists(featureFolder))
        {
            return HandleExtension.HandleError(messageService.GetMessage("Error Folder"));    
        }

        var files = Directory.GetFiles(featureFolder, "*.cs");
        if (files.Length == 0)
        {
            return HandleExtension.HandleError(messageService.GetMessage("Error File")+ files.Length +" archivos en "+ featureFolder );
        }

        return HandleExtension.HandleFinish(messageService.GetMessage("Finish Validate"));
    }
}