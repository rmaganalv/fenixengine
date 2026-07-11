// See https://aka.ms/new-console-template for more information
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
