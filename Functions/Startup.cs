using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Services;

[assembly: FunctionsStartup(typeof(Functions.Startup))]

namespace Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddLogging();
        builder.Services.AddSingleton<IImageService, ImageService>();
        builder.Services.AddSingleton<IColorService, ColorService>();
        builder.Services.AddSingleton<IDictionaryService, DictionaryService>();
    }
}