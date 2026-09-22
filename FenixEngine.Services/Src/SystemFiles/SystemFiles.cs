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
using System;
using System.IO;
using Service.Resources.Contracts;

namespace Service.Resources.SystemFiles;

public class SystemFiles: ISystemFiles
{

    public async Task<string>PickFile(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("Directorio no encontrado.");
            return string.Empty;
        }

        var files = Directory.GetFiles(directoryPath);
        if (files.Length == 0)
        {
            Console.WriteLine("No hay archivos en el directorio.");
            return string.Empty;
        }

        Console.WriteLine("Selecciona un archivo:");
        for (int i = 0; i < files.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
        }

        Console.Write("Ingresa el número: ");
        if (int.TryParse(Console.ReadLine(), out int choice) &&
            choice > 0 && choice <= files.Length)
        {
            return files[choice - 1];
        }

        Console.WriteLine("Selección inválida.");
        return string.Empty;
    }


    public async Task<string> ReadFileAsync(string filePath)
    {
  
        return await File.ReadAllTextAsync(filePath);
    }

    public async Task WriteFileAsync(string filePath, string content)
    {
        await File.WriteAllTextAsync(filePath, content);
    }

    public async Task<bool> FileExistsAsync(string filePath)
    {
        return await Task.FromResult(File.Exists(filePath));
    }

    public async Task<bool> DirectoryExistsAsync(string directoryPath)
    {
        return await Task.FromResult(Directory.Exists(directoryPath));
    }

    public async Task CreateDirectoryAsync(string directoryPath)
    {
        await Task.Run(() => Directory.CreateDirectory(directoryPath));
    }

    public async Task<string[]> GetFilesAsync(string directoryPath, string searchPattern)
    {
        return await Task.Run(() => Directory.GetFiles(directoryPath, searchPattern));
    }
}