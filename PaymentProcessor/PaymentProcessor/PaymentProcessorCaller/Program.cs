using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PaymentProcessor;
using Serilog;

public class Program
{
    public static async Task Main(string[] args)
    {
        var startup = new Startup();
        var services = new ServiceCollection();
        startup.ConfigureServices(services);

        Log.Logger = new LoggerConfiguration()
         .MinimumLevel.Debug()
         .WriteTo.Console()
         .CreateLogger();


        var serviceProvider = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);


        var busControl = serviceProvider.GetRequiredService<IBusControl>();
        await busControl.StartAsync();

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();

        await busControl.StopAsync();
    }
}