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
namespace FenixEngine.Shared.Src.Services;

public class ServiceFile: IFileService
{
    public async Task CreateFile(string? pathFile)
    {
        if (string.IsNullOrWhiteSpace(pathFile))
            throw new ArgumentException("La ruta del archivo no puede ser nula o vacía.");

        if (!File.Exists(pathFile))
            using (File.Create(pathFile)) { }
    }

    public async Task FindFile(string? pathFile)
    {
        if (string.IsNullOrWhiteSpace(pathFile))
            throw new ArgumentException("La ruta del archivo no puede ser nula o vacía.");

        if (!File.Exists(pathFile))
            throw new FileNotFoundException($"El archivo no existe: {pathFile}");
    }

    public async Task EditFile(string? pathFile, string? newContent)
    {
        if (string.IsNullOrWhiteSpace(pathFile))
            throw new ArgumentException("La ruta del archivo no puede ser nula o vacía.");

        if (!File.Exists(pathFile))
            throw new FileNotFoundException($"El archivo no existe: {pathFile}");

        File.WriteAllText(pathFile, newContent ?? string.Empty);
    }

    public async Task DeleteFile(string? pathFolder)
    {
        if (string.IsNullOrWhiteSpace(pathFolder))
            throw new ArgumentException("La ruta de la carpeta no puede ser nula o vacía.");

        if (Directory.Exists(pathFolder))
            Directory.Delete(pathFolder, recursive: true);
    }
}