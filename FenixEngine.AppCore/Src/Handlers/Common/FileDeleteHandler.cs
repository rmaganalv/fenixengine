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

using System.IO;
using System.Threading.Tasks;

namespace FenixEngine.AppCore.Nodes.Common;

public class FileCleanerHandler
{
    /// <summary>
    /// Elimina todos los archivos dentro del directorio especificado.
    /// Opcionalmente puede borrar también el directorio raíz.
    /// </summary>
    public async Task CleanDirectoryAsync(string targetDirectory, bool deleteRootDirectory = false)
    {
        if (!Directory.Exists(targetDirectory))
        {
            // Si no existe, no hay nada que limpiar, salimos silenciosamente o lanzamos según preferencia
            return; 
        }

        // 1. Obtener y borrar todos los archivos
        // Usamos Task.Run para no bloquear el hilo principal durante la operación de I/O síncrona de borrado
        var files = Directory.GetFiles(targetDirectory);
        
        foreach (var file in files)
        {
            await Task.Run(() => File.Delete(file));
        }

        // 2. Opcionalmente borrar subdirectorios (si los hubiera)
        var directories = Directory.GetDirectories(targetDirectory);
        foreach (var dir in directories)
        {
            await Task.Run(() => Directory.Delete(dir, true));
        }

        // 3. Opcionalmente borrar la carpeta raíz
        if (deleteRootDirectory)
        {
            // Pequeña espera para asegurar que el sistema de archivos liberó los handles
            await Task.Delay(50); 
            await Task.Run(() => 
            {
                if (Directory.Exists(targetDirectory))
                {
                    Directory.Delete(targetDirectory);
                }
            });
        }
    }
}   

    /*
    // Ejemplo de uso
string codigoConsolidado = await ReadAndConsolidateFilesAsync(@"C:\MiProyecto\Src", path => path.EndsWith(".cs"));   
    var cleaner = new FileCleanerHandler();

    // Solo vaciar la carpeta de salida
    await cleaner.CleanDirectoryAsync(@"C:\Generados\Output");

    // O borrar todo incluido el folder raíz
    await cleaner.CleanDirectoryAsync(@"C:\Generados\Output", deleteRootDirectory: true);   
    
    */

