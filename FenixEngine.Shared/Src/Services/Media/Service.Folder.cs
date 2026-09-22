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
    public class ServiceFolder: IFolderService
    {
        public async Task CreateFolder(string? pathFolder)
        {
            if (string.IsNullOrWhiteSpace(pathFolder))
                throw new ArgumentException("La ruta de la carpeta no puede ser nula o vacía.");

            if (!Directory.Exists(pathFolder))
                Directory.CreateDirectory(pathFolder);
        }

        public async Task DeleteFolder(string? pathFolder)
        {
            if (string.IsNullOrWhiteSpace(pathFolder))
                throw new ArgumentException("La ruta de la carpeta no puede ser nula o vacía.");

            if (Directory.Exists(pathFolder))
                Directory.Delete(pathFolder, recursive: true);
        }

        public async Task FindFolder(string? pathFolder)
        {
            if (string.IsNullOrWhiteSpace(pathFolder))
                throw new ArgumentException("La ruta de la carpeta no puede ser nula o vacía.");

            if (!Directory.Exists(pathFolder))
                throw new DirectoryNotFoundException($"La carpeta no existe: {pathFolder}");
        }

        public async Task EditFolder(string? pathFolder, string? newPathFolder)
        {
            if (string.IsNullOrWhiteSpace(pathFolder) || string.IsNullOrWhiteSpace(newPathFolder))
                throw new ArgumentException("Las rutas no pueden ser nulas o vacías.");

            if (!Directory.Exists(pathFolder))
                throw new DirectoryNotFoundException($"La carpeta no existe: {pathFolder}");

            Directory.Move(pathFolder, newPathFolder);
        }


    }


