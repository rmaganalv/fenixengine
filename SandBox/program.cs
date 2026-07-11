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
using AppCore.Generator;
using AppCore.Generator.Generators;
public class program
{
    public static async Task Main(string[] args)
    {
        States currentState = States.Idle;
 
        while (true)
        {
            switch (currentState)
            {
               case States.Idle:
                    currentState = await Orchestra.HandleIdle();
                    break;

                case States.Generating:
                    currentState = await Orchestra.HandleGenerating();
                    break;

                 case States.Validating:
                    currentState = await Orchestra.HandleValidating();
                    break;

                case States.Completed:
                   currentState = await Orchestra.HandleCompleted();
                    return;

                case States.Error:
                    currentState = await Orchestra.HandleError();
                    return;
            }
        }
        
    }
}
