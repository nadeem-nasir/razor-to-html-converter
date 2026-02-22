namespace AzureFuncs.RazorToHtmlConverterFunctions.Extensions;
public static class StartupExtensions
{
    /// <summary>
    /// an extension method for the IServiceCollection interface. 
    /// It adds the necessary services and configurations to enable Razor pages to be converted to HTML. 
    /// without using a controller in a non-web context.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRazorToHtmlConverter(this IServiceCollection services)
    {
        services
         .AddMvcCore() //AddMvcCore registers all the required core services to the MVC application. It adds the minimum essential MVC services to the specified IServiceCollection, additional services including MVC’s support for authorization, formatters, and validation must be added separately using the AddAuthorization, AddMvcFormatters, and AddDataAnnotations methods.
         .AddApplicationPart(Assembly.GetExecutingAssembly()) //An ApplicationPart is an abstraction over the resources of an app, such as controllers, views, Razor Pages, tag helpers, etc. By adding an ApplicationPart, you can configure your app to discover and load ASP.NET Core features from an assembly.
         .AddRazorRuntimeCompilation(options =>
         {
             options.FileProviders.Clear();
             options.FileProviders.Add(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
         });

        services.AddTransient<IRazorToStringRendererService, RazorToStringRendererService>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ISimulatorService, SimulatorService>();

        return services;

    }

}