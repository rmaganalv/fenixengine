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
public abstract class ControllerBase<TResult>
{
    // Pasos abstractos/virtuales que las clases hijas DEBEN o PUEDEN personalizar
    protected abstract Task OnBeforeExecuteAsync(string featureName);
    protected abstract Task<TResult> OnRunAsync(string featureName);
    protected abstract TResult OnError(Exception exception);

    // Método Plantilla (Template Method): Define el flujo fijo e inalterable
    public async Task<TResult>ExecuteAsync(TResult statesGenerator)
    {
        LogStart(statesGenerator);

        try
        {
            LogStep(statesGenerator,"");
            await OnBeforeExecuteAsync("Iniciando preparación del entorno...");

            LogStep(statesGenerator, "");
            var result = await OnRunAsync("Ejecutando proceso principal de generación...");

            LogFinish();
            return result;
        }
        catch (Exception ex)
        {
            LogError(ex.Message);
            return OnError(ex);
        }
    }


    // Método protegido para que las clases hijas reporten avances intermedios si lo necesitan
    protected void LogStep(TResult stateGenerator, string message)
    {
        Console.WriteLine($"[PROCESO] -> {stateGenerator}: {message}");
    }

    private void LogStart(TResult stateGenerator)
    {
        Console.WriteLine($"\n==================================================");
        Console.WriteLine($"[INICIO] Ejecutando proceso: {stateGenerator}");
        Console.WriteLine($"==================================================");
    }


    private void LogError(string errorMessage)
    {
        Console.WriteLine($"\n[ERROR CRÍTICO] Fallo durante la ejecución: {errorMessage}");
    }


    private void LogFinish()
    {
        Console.WriteLine($"\n==================================================");
        Console.WriteLine($"[FIN] Proceso finalizado correctamente.");
        Console.WriteLine($"==================================================\n");
    }

}