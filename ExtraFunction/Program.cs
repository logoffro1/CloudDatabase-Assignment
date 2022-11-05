using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ExtraFunction.Repository.Interface;
using ExtraFunction.DAL;
using ExtraFunction.Service;
using ExtraFunction.Authorization;
using ExtraFunction.Repository;

namespace ExtraFunction
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                    .ConfigureFunctionsWorkerDefaults(Worker => Worker.UseNewtonsoftJson().UseMiddleware<JWTMiddleware>())
                    .ConfigureAppConfiguration(config =>
                         config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: false))
                    .ConfigureOpenApi()
                    .ConfigureServices(services =>
                    {
                        services.AddControllers();
                        services.AddDbContext<DatabaseContext>(options =>
                                                            options.UseCosmos("https://cosmin.documents.azure.com:443/",
               "yEWSz1XH7ys7y44CdOkEaBkuha7tSwXk1TS5XoJKHVOn6qV08J8VpXbeF19YyXBk8WXiTZabILaoCXzPXIXDJw==",
               "cloud-homework"));
                        services.AddTransient<IBlobStorageService, BlobStorageService>();
                        services.AddTransient<IBlobStorageRepository, BlobStorageRepository>();

                    })
                    .Build();
            await host.RunAsync();

            
        }
    }
}
