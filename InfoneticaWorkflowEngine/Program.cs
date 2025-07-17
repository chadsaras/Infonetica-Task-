using InfoneticaWorkflowEngine.Models;
using InfoneticaWorkflowEngine.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var service = new WorkflowService();

app.MapPost("/workflow", (WorkflowDefinition def) =>
{
    if (!service.ValidateDefinition(def, out var error))
        return Results.BadRequest(error);

    service.Definitions.Add(def);
    return Results.Ok(def);
});

app.MapGet("/workflow/{id}", (string id) =>
{
    var def = service.GetDefinition(id);
    return def is null ? Results.NotFound() : Results.Ok(def);
});

app.MapPost("/workflow/{id}/start", (string id) =>
{
    var def = service.GetDefinition(id);
    if (def is null) return Results.NotFound("Workflow definition not found.");

    var instance = service.StartInstance(id);
    return Results.Ok(instance);
});

app.MapPost("/instance/{id}/action/{actionId}", (string id, string actionId) =>
{
    var (success, error) = service.ExecuteAction(id, actionId);
    return success ? Results.Ok("Transition successful.") : Results.BadRequest(error);
});

app.MapGet("/instance/{id}", (string id) =>
{
    var inst = service.GetInstance(id);
    return inst is null ? Results.NotFound() : Results.Ok(inst);
});
app.MapGet("/instances", () =>
{
    var instancesWithState = service.Instances.Select(instance => new
    {
        InstanceId = instance.Id,
        WorkflowId = instance.WorkflowDefinitionId,
        CurrentState = instance.CurrentStateId,
        History = instance.History
    });

    return Results.Ok(instancesWithState);
});

app.MapGet("/", () => "Workflow Engine is running.");
app.Run();
