using Xunit;
using Microsoft.AspNetCore.Mvc;
using Elsa.Aspire.Server.Controllers;

namespace Elsa.Aspire.Tests;

public class WorkflowControllerTests
{
    [Fact]
    public void GetWorkflowDefinitions_ShouldReturnWorkflowInformation()
    {
        var controller = new WorkflowController();

        var result = controller.GetWorkflowDefinitions();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value?.ToString();

        Assert.Contains("Workflow definitions endpoint", response);
    }

    [Fact]
    public void GetWorkflowStatus_ShouldReturnOperationalStatus()
    {
        var controller = new WorkflowController();

        var result = controller.GetWorkflowStatus();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value?.ToString();

        Assert.Contains("Workflow runtime is operational", response);
    }

    [Fact]
    public async Task TriggerWorkflow_ShouldReturnSuccessResponse()
    {
        var controller = new WorkflowController();

        var result = await controller.TriggerWorkflow();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value?.ToString();

        Assert.Contains("Workflow trigger endpoint available", response);
    }

    [Fact]
    public void WorkflowController_ShouldHaveProperControllerAttributes()
    {
        var controllerType = typeof(WorkflowController);

        Assert.True(controllerType.IsSubclassOf(typeof(ControllerBase)));

        var apiControllerAttribute = controllerType.GetCustomAttributes(typeof(ApiControllerAttribute), false);
        Assert.NotEmpty(apiControllerAttribute);

        var routeAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), false);
        Assert.NotEmpty(routeAttribute);
    }

    [Fact]
    public void WorkflowController_ShouldHaveCorrectEndpoints()
    {
        var controllerType = typeof(WorkflowController);
        var methods = controllerType.GetMethods();

        var definitionsMethod = methods.FirstOrDefault(m => m.Name == "GetWorkflowDefinitions");
        Assert.NotNull(definitionsMethod);

        var statusMethod = methods.FirstOrDefault(m => m.Name == "GetWorkflowStatus");
        Assert.NotNull(statusMethod);

        var triggerMethod = methods.FirstOrDefault(m => m.Name == "TriggerWorkflow");
        Assert.NotNull(triggerMethod);

        var testMethod = methods.FirstOrDefault(m => m.Name == "TestWorkflowExecution");
        Assert.NotNull(testMethod);
    }

    [Fact]
    public void WorkflowController_Methods_ShouldHaveCorrectHttpAttributes()
    {
        var controllerType = typeof(WorkflowController);

        var definitionsMethod = controllerType.GetMethod("GetWorkflowDefinitions");
        Assert.NotNull(definitionsMethod);
        var httpGetAttr = definitionsMethod.GetCustomAttributes(typeof(HttpGetAttribute), false);
        Assert.NotEmpty(httpGetAttr);

        var statusMethod = controllerType.GetMethod("GetWorkflowStatus");
        Assert.NotNull(statusMethod);
        var statusGetAttr = statusMethod.GetCustomAttributes(typeof(HttpGetAttribute), false);
        Assert.NotEmpty(statusGetAttr);

        var triggerMethod = controllerType.GetMethod("TriggerWorkflow");
        Assert.NotNull(triggerMethod);
        var postAttr = triggerMethod.GetCustomAttributes(typeof(HttpPostAttribute), false);
        Assert.NotEmpty(postAttr);

        var testMethod = controllerType.GetMethod("TestWorkflowExecution");
        Assert.NotNull(testMethod);
        var testGetAttr = testMethod.GetCustomAttributes(typeof(HttpGetAttribute), false);
        Assert.NotEmpty(testGetAttr);
    }
}
