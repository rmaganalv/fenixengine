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

using FenixEngine.Shared.Src.Utils;

namespace FenixEngine.Shared.Src.Services
{
    public interface IFileService
    {
        Task CreateFile(string? pathFile);
        Task DeleteFile(string? pathFile);
        Task FindFile(string? pathFile);
        Task EditFile(string? pathFile, string? newContent);
    }

    public interface IFolderService
    {
        Task CreateFolder(string? pathFolder);
        Task DeleteFolder(string? pathFolder);
        Task FindFolder(string? pathFolder);
        Task EditFolder(string? pathFolder, string? newPathFolder);

    } 

    public interface ITerminalService
    {
        Task<string> ExecuteCommandAsync(string? command);
    }

    public interface ILoggerService
    {
        void Log(LogLevel level, string message, string? correlationId = null);
        void Info(string message, string? correlationId = null);
        void Warning(string message, string? correlationId = null);
        void Error(string message, string? correlationId = null);
        void Debug(string message, string? correlationId = null);
    }


}
