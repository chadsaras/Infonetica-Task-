namespace InfoneticaWorkflowEngine.Models;

public class WorkflowDefinition
{
    public string Id { get; set; } = default!;
    public List<State> States { get; set; } = new();
    public List<ActionDefinition> Actions { get; set; } = new();
}
