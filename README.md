# Configurable Workflow Engine

This project is a minimal, extensible **state-machine based workflow engine** built with **.NET 8 Minimal APIs**. It allows users to define workflows as configurable state machines, create instances of these workflows, and execute transitions (actions) between states with full validation.

---

## Core Functionalities

- **Define Workflows:** Create workflow definitions consisting of states and actions.
- **Incrementally Update Workflows:** Add new states and actions after initial definition.
- **Start Instances:** Launch workflow instances that follow a defined workflow.
- **Execute Actions:** Move instances from one state to another via valid transitions.
- **Validation Logic:** 
  - Only one initial state is allowed
  - Transitions are only allowed from valid, enabled states and actions
  - No transitions from final states
- **Track History:** Maintain a basic history of all executed actions per instance.
- **Introspect State:** Retrieve the current state and available actions for an instance.
- **In-Memory Persistence:** Uses memory-based storage (no database).
- **API-First Design:** Fully accessible and testable via REST API.

---

## Implementation Details

- **Platform:** .NET 8 (C# 12), ASP.NET Core Minimal API
- **Folder Structure:**
  - `Models/` contains data classes like `State`, `ActionDefinition`, `WorkflowDefinition`, `WorkflowInstance`
  - `Services/` contains the main `WorkflowService` logic handling core engine functionality
  - `Program.cs` defines all routes using minimal API routing
- **Architecture:**
  - Stateless controller logic with `WorkflowService` acting as the in-memory engine
  - Fully decoupled data model and service logic
  - Clean, flat route design following RESTful conventions
- **Validation:** Performed during workflow creation and action execution to enforce state machine integrity

---

## How to Run

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Steps

1. **Clone the repository** (if applicable) or navigate to the project folder.

2. **Run the application**:

   ```bash
   dotnet run
