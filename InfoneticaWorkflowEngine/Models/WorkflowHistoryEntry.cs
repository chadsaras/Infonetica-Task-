namespace InfoneticaWorkflowEngine.Models;

public class WorkflowHistoryEntry
{
    public string ActionId { get; set; } = default!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
