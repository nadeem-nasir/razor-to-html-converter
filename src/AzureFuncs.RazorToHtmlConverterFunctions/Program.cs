var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();
    })
    // use ConfigureFunctionsWebApplication() instead of ConfigureFunctionsWorkerDefaults() for ASP.NET Core integration.
    //ASP.NET Core integration make it possible to use the underlying HTTP request and response objects using types from ASP.NET Core including HttpRequest, HttpResponse, and IActionResult. 
    .ConfigureFunctionsWebApplication() 
     .ConfigureServices((hostContext, services) =>
     {
         services.AddApplicationInsightsTelemetryWorkerService();
         services.ConfigureFunctionsApplicationInsights();
         services.AddServices();
         services.AddRazorToHtmlConverter();
     })
    .Build();

await host.RunAsync();
