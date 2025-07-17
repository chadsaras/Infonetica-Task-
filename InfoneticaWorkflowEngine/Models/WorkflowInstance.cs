namespace InfoneticaWorkflowEngine.Models;

public class WorkflowInstance
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string WorkflowDefinitionId { get; set; } = default!;
    public string CurrentStateId { get; set; } = default!;
    public List<WorkflowHistoryEntry> History { get; set; } = new();
}
