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

namespace FenixEngine.AppCore.Utils.Contracts;

/// <summary>
/// 
/// </summary>
public abstract class NodeBase<TResult>
{
    // Pasos abstractos/virtuales que las clases hijas DEBEN o PUEDEN personalizar
    protected abstract Task<TResult> OnBeforeExecuteAsync(string startNode);
    protected abstract Task<TResult> OnExecuteAsync(string processNode);
    protected abstract Task<TResult> OnError(Exception exception);

    // Método Plantilla (Template Method): Define el flujo fijo e inalterable
    public async Task<TResult> NodeRunAsync(string featureName)
    {
        LogStart(featureName);

        try
        {
            LogStep("Preparing...");
            await OnBeforeExecuteAsync(featureName);

            LogStep("Executing main node... ");
            var result = await OnExecuteAsync(featureName);

            LogFinish();
            return result;
        }
        catch (Exception ex)
        {
            LogError(ex.Message);
            return await OnError(ex);
        }
    }


    // Método protegido para que las clases hijas reporten avances intermedios si lo necesitan
    protected void LogStep(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[PROCESS] -> {message}");
        Console.ResetColor();
    }

    private void LogStart(string featureName)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[START] Begin node process: {featureName}");
        Console.ResetColor();
    }


    private void LogError(string errorMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n[CRITICAL ERROR] Process fail: {errorMessage}");
        Console.ResetColor();
    }


    private void LogFinish()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[FINISH] NOde process is end.");
        Console.ResetColor();
    }

}