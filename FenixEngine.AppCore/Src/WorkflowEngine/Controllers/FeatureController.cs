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
using System.Globalization;
using FenixEngine.AppCore.Executions;
using FenixEngine.AppCore.Utils.Enums;


namespace FenixEngine.AppCore.Machines;

public static class FeatureController
{
    public static async Task FeatureHandle()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("es"); 
        StatesGenerator currentState = StatesGenerator.Idle;
 
        while (true)
        {
            switch (currentState)
            {
               case StatesGenerator.Idle:
                    currentState = await HandleIdleExecution.HandleIdle(); // Orchestra.HandleIdle();
                    break;

                case StatesGenerator.Generating:
                    currentState = await HandleGeneratingExecution.HandleGenerating();//Orchestra.HandleGenerating();
                    break;

                 case StatesGenerator.Validating:
                    currentState = await HandleValidated.HandleValidating(); //Orchestra.HandleValidating();
                    break;

                case StatesGenerator.Completed:
                   currentState = await HandleCompletedExecution.HandleCompleted();//Orchestra.HandleCompleted();
                    return;

                case StatesGenerator.Error:
                    currentState = await HandleErrorExecution.HandleError();//Orchestra.HandleError();
                    return;
            }
        }
    }
}