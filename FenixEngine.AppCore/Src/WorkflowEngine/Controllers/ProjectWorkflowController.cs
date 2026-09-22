namespace FenixEngine.AppCore.Src.WorkflowEngine.Controllers;

public enum ProjectWorkflowState
{
    Idle,
    Prompting,
    Generating,
    Validating,
    Completed,
    Error
}

public class ProjectWorkflowController
{
    public ProjectWorkflowState State { get; private set; } = ProjectWorkflowState.Idle;

    public async Task<ProjectWorkflowState> ExecuteAsync(string featureName)
    {
        State = ProjectWorkflowState.Prompting;
        await Task.Delay(50);

        State = ProjectWorkflowState.Generating;
        await Task.Delay(50);

        State = ProjectWorkflowState.Validating;
        await Task.Delay(50);

        State = ProjectWorkflowState.Completed;
        return State;
    }
}
