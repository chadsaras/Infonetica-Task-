using InfoneticaWorkflowEngine.Models;

namespace InfoneticaWorkflowEngine.Services;

public class WorkflowService
{
    public List<WorkflowDefinition> Definitions { get; } = new();
    public List<WorkflowInstance> Instances { get; } = new();

    public WorkflowDefinition? GetDefinition(string id) =>
        Definitions.FirstOrDefault(d => d.Id == id);

    public WorkflowInstance? GetInstance(string id) =>
        Instances.FirstOrDefault(i => i.Id == id);

    public bool ValidateDefinition(WorkflowDefinition def, out string error)
    {
        var stateIds = new HashSet<string>();
        if (def.States.Count(s => s.IsInitial) != 1)
        {
            error = "Definition must have exactly one initial state.";
            return false;
        }

        foreach (var s in def.States)
        {
            if (!stateIds.Add(s.Id))
            {
                error = $"Duplicate state ID: {s.Id}";
                return false;
            }
        }

        foreach (var a in def.Actions)
        {
            if (!def.States.Any(s => s.Id == a.ToState))
            {
                error = $"Action {a.Id} has unknown toState: {a.ToState}";
                return false;
            }

            foreach (var from in a.FromStates)
            {
                if (!def.States.Any(s => s.Id == from))
                {
                    error = $"Action {a.Id} has unknown fromState: {from}";
                    return false;
                }
            }
        }

        error = "";
        return true;
    }

    public WorkflowInstance StartInstance(string definitionId)
    {
        var def = GetDefinition(definitionId)!;
        var init = def.States.First(s => s.IsInitial);
        var instance = new WorkflowInstance
        {
            WorkflowDefinitionId = def.Id,
            CurrentStateId = init.Id
        };
        Instances.Add(instance);
        return instance;
    }

    public (bool success, string error) ExecuteAction(string instanceId, string actionId)
    {
        var instance = GetInstance(instanceId);
        if (instance == null) return (false, "Instance not found.");

        var def = GetDefinition(instance.WorkflowDefinitionId)!;
        var action = def.Actions.FirstOrDefault(a => a.Id == actionId);
        if (action == null) return (false, "Action not found.");
        if (!action.Enabled) return (false, "Action is disabled.");
        if (!action.FromStates.Contains(instance.CurrentStateId)) return (false, "Invalid transition.");

        var toState = def.States.FirstOrDefault(s => s.Id == action.ToState);
        if (toState == null || !toState.Enabled) return (false, "Target state invalid/disabled.");

        if (def.States.First(s => s.Id == instance.CurrentStateId).IsFinal)
            return (false, "Current state is final.");

        instance.CurrentStateId = toState.Id;
        instance.History.Add(new WorkflowHistoryEntry { ActionId = action.Id });
        return (true, "");
    }
}
