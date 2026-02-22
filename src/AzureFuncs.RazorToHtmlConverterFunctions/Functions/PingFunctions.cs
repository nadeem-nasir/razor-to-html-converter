namespace AzureFuncs.RazorToHtmlConverterFunctions.Functions;
public class PingFunctions(ILoggerFactory loggerFactory, IRazorToStringRendererService razorToStringRendererService, ISimulatorService simulatorService)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<PingFunctions>();
    private readonly IRazorToStringRendererService _razorToStringRendererService = razorToStringRendererService ?? throw new ArgumentNullException(nameof(razorToStringRendererService), "The razorToStringRendererService service  is required");
    private readonly ISimulatorService _simulatorService = simulatorService ?? throw new ArgumentNullException(nameof(simulatorService), "The simulatorService service  is required");

    /// <summary>
    /// Demo function to show how to convert a razor view to html string in non-web context
    /// Without using any third party library or open source nuget package
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [Function(nameof(PingProductsSale))]
    public async Task<HttpResponseData> PingProductsSale([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        var productsSales = _simulatorService.GetProductsSale(20);

        var htmlInvoice = await _razorToStringRendererService.RenderRazorToStringAsync("Views/ProductsSale.cshtml", model: productsSales, isPartial: false);

        var response = req.CreateResponse(HttpStatusCode.OK);

        response.Headers.Add("Content-Type", "text/html; charset=utf-8");

        await response.WriteStringAsync(htmlInvoice);

        return response;
    }
}
