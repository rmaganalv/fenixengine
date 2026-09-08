using FenixEngine.AppCore.Src.Contracts;
using FenixEngine.AppCore.Src.Orchestration;
/*
var context = new ServiceContext();
context.Parameters["path"] = "/Users/Ruben/Projects";
context.Parameters["prompt"] = "Generar clase de repositorio";

var root = new DecisionNode
{
    Handler = new FindFolderHandler(),
    Next = new List<DecisionNode>
    {
        new DecisionNode
        {
            Handler = new GenerateCodeHandler(),
            Condition = ctx => !string.IsNullOrEmpty(ctx.GetParameter<string>("path")),
            Next = new List<DecisionNode>
            {
                new DecisionNode { Handler = new SaveToDatabaseHandler() }
            }
        }
    }
};

var engine = new DecisionTreeEngine();
await engine.ExecuteAsync(root, context);*/
