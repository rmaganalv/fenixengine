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
using FenixEngine.Shared.Src.Services;
using FenixEngine.Shared.Src.Utils;

namespace FenixEngine.Shared.Modules
{
    public class ServiceFileLogger : ILoggerService
    {
        private readonly string _logFilePath;

        public ServiceFileLogger()
        {
            // Ruta multiplataforma: carpeta de datos de la app
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _logFilePath = Path.Combine(basePath, "logs", "app.log");

            string? folder = Path.GetDirectoryName(_logFilePath);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        private void WriteLog(string level, string message, string? correlationId)
        {
            string id = string.IsNullOrWhiteSpace(correlationId) ? Guid.NewGuid().ToString() : correlationId;
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] [CID:{id}] {message}";

            // Prioridad: si es Error, lo escribimos primero en consola además de archivo
            if (level == "ERROR")
                Console.Error.WriteLine(logEntry);

            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }

        public void Log(LogLevel level, string message, string? correlationId = null)
            => WriteLog(level.ToString().ToUpper(), message, correlationId);

        public void Info(string message, string? correlationId = null) => Log(LogLevel.Info, message, correlationId);
        public void Warning(string message, string? correlationId = null) => Log(LogLevel.Warning, message, correlationId);
        public void Error(string message, string? correlationId = null) => Log(LogLevel.Error, message, correlationId);
        public void Debug(string message, string? correlationId = null) => Log(LogLevel.Debug, message, correlationId);
    }
}