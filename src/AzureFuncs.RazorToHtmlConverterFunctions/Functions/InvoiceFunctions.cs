namespace AzureFuncs.RazorToHtmlConverterFunctions.Functions;

public class InvoiceFunctions(ILoggerFactory loggerFactory, IRazorToStringRendererService razorToStringRendererService, ISimulatorService simulatorService)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<InvoiceFunctions>();
    private readonly IRazorToStringRendererService _razorToStringRendererService = razorToStringRendererService ?? throw new ArgumentNullException(nameof(razorToStringRendererService), "The razorToStringRendererService service  is required");
    private readonly ISimulatorService _simulatorService = simulatorService ?? throw new ArgumentNullException(nameof(simulatorService), "The simulatorService service  is required");

    [Function(nameof(InvoiceProcessingStep2ToHtml))]
    [BlobOutput("invoice-processing-step2-as-html-data/{name}.html", Connection = "AzureWebJobsStorage")]
    public async Task<string> InvoiceProcessingStep2ToHtml([BlobTrigger("invoice-processing-step1-as-json-data/{name}", Connection = "AzureWebJobsStorage")] InvoiceViewModel invoiceViewModel)
    {
        var htmlInvoice = await _razorToStringRendererService.RenderRazorToStringAsync("Views/InvoiceDetailPreview.cshtml", model: invoiceViewModel, isPartial: false);

        return htmlInvoice;
    }
}