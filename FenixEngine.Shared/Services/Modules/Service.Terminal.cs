using System.Diagnostics;

namespace FenixEngine.Shared.Services
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
