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
using System.Diagnostics;

namespace FenixEngine.Shared.Src.Services
{
    public class ServiceTerminal: ITerminalService
    {
        public async Task<string> ExecuteCommandAsync(string? command)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentException("El comando no puede ser nulo o vacío.");

            string fileName;
            string arguments;

            if (OperatingSystem.IsWindows())
            {
                fileName = "powershell.exe";
                arguments = $"-Command \"{command}\"";
            }
            else
            {
                fileName = "/bin/bash";
                arguments = $"-c \"{command}\"";
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await Task.Run(() => process.WaitForExit());

            return string.IsNullOrEmpty(error) ? output : $"Error: {error}";
        }
    }
}
