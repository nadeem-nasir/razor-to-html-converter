using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace AzureFuncs.RazorToHtmlConverterFunctions.Services;
public interface IRazorToStringRendererService
{
    // A method to render a razor view with a given model to a string asynchronously in non-Web
    Task<string> RenderRazorToStringAsync<TModel>(string partialName, TModel model, bool isPartial = false);
}
/// <summary>
/// Render a razor view to a html string in non-Web.
/// Adapted from:
/// https://github.com/aspnet/Entropy/blob/93ee2cf54eb700c4bf8ad3251f627c8f1a07fb17/samples/Mvc.RenderViewToString/RazorViewToStringRenderer.cs
/// </summary>
/// <param name="viewEngine"></param>
/// <param name="tempDataProvider"></param>
/// <param name="serviceProvider"></param>
public class RazorToStringRendererService(IRazorViewEngine viewEngine,
ITempDataProvider tempDataProvider,
IServiceProvider serviceProvider) : IRazorToStringRendererService
{
    // Private fields for the dependencies
    private readonly IRazorViewEngine _viewEngine = viewEngine ?? throw new ArgumentNullException(nameof(viewEngine));
    private readonly ITempDataProvider _tempDataProvider = tempDataProvider ?? throw new ArgumentNullException(nameof(tempDataProvider));
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task<string> RenderRazorToStringAsync<TModel>(string viewNameOrPath, TModel model, bool isPartial)
    {
        // Get the action context from the current HTTP context
        var actionContext = GetActionContext();
        viewNameOrPath = ResolveToRelativePath(actionContext, viewNameOrPath);

        // Find the partial view using the view engine
        var partial = FindView(actionContext, viewNameOrPath, isPartial);

        // Create a string writer to store the output
        using var output = new StringWriter();
        // Create a view context with the model data and output writer
        var viewContext = new ViewContext(
        actionContext,
        partial,
        new ViewDataDictionary<TModel>(
        metadataProvider: new EmptyModelMetadataProvider(),
        modelState: new ModelStateDictionary())
        {
            Model = model
        },
        new TempDataDictionary(
        actionContext.HttpContext,
        _tempDataProvider),
        output,
        new HtmlHelperOptions()
        );

        // Render the partial view asynchronously and write to the output
        await partial.RenderAsync(viewContext);

        // Return the output as a string
        return output.ToString();
    }

    // A private helper method to find a view by its name or relative path
    private IView FindView(ActionContext actionContext, string viewNameOrPath, bool isPartial)
    {
        // Declare a variable to store the view result
        ViewEngineResult viewResult;
        // Try to get the view by its name without specifying the path
        viewResult = _viewEngine.GetView(null, viewNameOrPath, isPartial);

        // If the view is not found, try to find the view by its name in the current action context
        if (!viewResult.Success)
        {
            viewResult = _viewEngine.FindView(actionContext, viewNameOrPath, isPartial);
        }

        // If the view is found, return it
        if (viewResult.Success)
        {
            return viewResult.View;
        }

        // Otherwise, throw an exception with an error message using string interpolation and string.Join
        throw new InvalidOperationException($"Unable to find view '{viewNameOrPath}'. The following locations were searched: {string.Join(Environment.NewLine, viewResult.SearchedLocations)}");
    }

    private static string ResolveToRelativePath(ActionContext actionContext, string viewNameOrPath)
    {
        var resolvedPath = viewNameOrPath.StartsWith('~') || viewNameOrPath.StartsWith('/')
        // If the view name or path starts with a tilde (~) or a slash (/), which indicates a relative path
        // Resolve the relative path using the UrlHelper
        ? new UrlHelper(actionContext).Content(viewNameOrPath)
        // If the view name or path does not start with a tilde (~) or a slash (/), which indicates a normal name
        // Return the view name or path as it is
        : viewNameOrPath;

        return resolvedPath;
    }

    private ActionContext GetActionContext()
    {
        // Get the current HTTP context from the service provider
        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };

        // Return a new action context with the HTTP context, route data and action descriptor
        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }

}

