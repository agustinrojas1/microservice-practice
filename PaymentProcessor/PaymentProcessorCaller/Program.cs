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

        var serviceProvider = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

        var busControl = serviceProvider.GetRequiredService<IBusControl>();
        await busControl.StartAsync();

        Console.CancelKeyPress += async (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;

            await busControl.StopAsync();

            Environment.Exit(0);
        };

        Console.WriteLine("Presiona Ctrl+C para salir.");

        await Task.Delay(Timeout.Infinite);

    }
}