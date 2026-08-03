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
namespace Service.Resources.Contracts;


public interface ISystemFiles
{
    Task<string>PickFile(string directoryPath);
    Task<string> ReadFileAsync(string filePath);
    Task WriteFileAsync(string filePath, string content);
    Task<bool> FileExistsAsync(string filePath);
    Task<bool> DirectoryExistsAsync(string directoryPath);
    Task CreateDirectoryAsync(string directoryPath);
    Task<string[]> GetFilesAsync(string directoryPath, string searchPattern);
}