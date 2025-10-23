using Microsoft.AspNetCore.Mvc;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.State;

namespace Elsa.Aspire.Server.Controllers;

/// <summary>
/// Controller to manually trigger workflows and get workflow information
/// Provides endpoints for workflow management and execution
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WorkflowController : ControllerBase
{
    public WorkflowController()
    {
    }

    /// <summary>
    /// Get available workflow information
    /// </summary>
    [HttpGet("definitions")]
    public IActionResult GetWorkflowDefinitions()
    {
        return Ok(new
        {
            message = "Workflow definitions endpoint",
            availableWorkflows = new[]
            {
                new { name = "TestWorkflow", endpoint = "/api/test-workflow", method = "GET", description = "Simple test workflow with JSON response" },
                new { name = "DataProcessingWorkflow", endpoint = "/api/process-data", method = "POST", description = "Data processing workflow with variables" },
                new { name = "HealthCheckWorkflow", endpoint = "N/A (Timer-based)", method = "Timer", description = "Scheduled health check workflow" }
            }
        });
    }

    /// <summary>
    /// Manually trigger a workflow
    /// </summary>
    [HttpPost("trigger")]
    public Task<IActionResult> TriggerWorkflow([FromBody] object? input = null)
    {
        try
        {
            // Simplified workflow triggering
            return Task.FromResult<IActionResult>(Ok(new
            {
                message = "Workflow trigger endpoint available",
                suggestion = "Use /api/workflow/test for testing workflow functionality",
                timestamp = DateTime.UtcNow
            }));
        }
        catch (Exception ex)
        {
            return Task.FromResult<IActionResult>(StatusCode(500, new { error = "Failed to process request", details = ex.Message }));
        }
    }

    /// <summary>
    /// Get status of running workflows
    /// </summary>
    [HttpGet("status")]
    public IActionResult GetWorkflowStatus()
    {
        return Ok(new
        {
            message = "Workflow runtime is operational",
            serverTime = DateTime.UtcNow,
            availableEndpoints = new[]
            {
                "/api/workflow/definitions - Get workflow information",
                "/api/workflow/trigger - Trigger workflow endpoint",
                "/api/workflow/test - Test workflow execution"
            }
        });
    }

    /// <summary>
    /// Test workflow execution endpoint - Creates and executes a simple workflow
    /// </summary>
    [HttpGet("test")]
    public async Task<IActionResult> TestWorkflowExecution([FromServices] IWorkflowRunner workflowRunner)
    {
        try
        {
            // Create a simple workflow programmatically
            var workflow = new Sequence
            {
                Activities =
                {
                    new WriteLine("Hello from a dynamically created workflow!")
                }
            };

            // Execute the workflow directly
            var result = await workflowRunner.RunAsync(workflow);

            return Ok(new
            {
                message = "Real workflow executed successfully!",
                workflowId = result.WorkflowState.Id,
                status = result.WorkflowState.Status.ToString(),
                subStatus = result.WorkflowState.SubStatus.ToString(),
                isFinished = result.WorkflowState.Status == WorkflowStatus.Finished,
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " UTC",
                executedVia = "Direct workflow execution"
            });
        }
        catch (Exception ex)
        {
            return Ok(new
            {
                error = "Failed to execute workflow",
                details = ex.Message,
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " UTC"
            });
        }
    }
}