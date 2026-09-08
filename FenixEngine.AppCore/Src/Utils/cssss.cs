/*var root = new ConditionalNode
{
    Handler = new DetectProjectHandler(),
    Next = new List<ConditionalNode>
    {
        new ConditionalNode
        {
            Handler = new GenerateArchitectureHandler(),
            Condition = ctx => ctx.GetParameter<bool>("projectExists")
        },
        new ConditionalNode
        {
            Handler = new CreateNewProjectHandler(),
            Condition = ctx => !ctx.GetParameter<bool>("projectExists")
        }
    }
};*/
