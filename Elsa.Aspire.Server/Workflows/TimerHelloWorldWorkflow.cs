using Elsa.Workflows;
using Elsa.Workflows.Activities;

namespace Elsa.Aspire.Server.Workflows;

public class TimerHelloWorldWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Name = "Timer Hello World";
        builder.Root = new Sequence
        {
            Activities =
            {
                new Elsa.Scheduling.Activities.Timer(TimeSpan.FromSeconds(15))
                {
                    CanStartWorkflow = true
                },
                new WriteLine("Hello World from the Elsa timer workflow.")
            }
        };
    }
}
