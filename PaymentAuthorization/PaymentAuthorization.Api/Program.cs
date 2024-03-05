using MassTransit;
using PaymentAuthorization.Api.AsyncDataServices.Publisher;
using PaymentAuthorization.Api.Data.Repository;
using PaymentAuthorization.Api.Services;
using PaymentProcessorCaller.AsyncDataServices.Consumer;
using PaymentAuthorization.Api.Data.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;


class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IPaymentAuthorizationService, PaymentAuthorizationService>();
        builder.Services.AddSingleton<IMessageBusPublisherClient, MessageBusPublisherClient>();
        builder.Services.AddScoped<IPaymentAuthorizationRepository, PaymentAuthorizationRepository>();
        builder.Services.AddScoped<IAcceptedAuthorizationRepository, AcceptedAuthorizationRepository>();

        builder.Services.AddDbContext<AuthorizationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AuthorizationDb")));

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddLogging(builder =>
        {
            builder.AddSerilog();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        builder.Services.AddMassTransit(x =>
        {
            var rabbitMqOptions = builder.Configuration.GetSection("RabbitMQ");
            var host = rabbitMqOptions["Host"];
            var port = int.Parse(rabbitMqOptions["Port"]);
            var username = rabbitMqOptions["Username"];
            var password = rabbitMqOptions["Password"];
            var responseQueue = rabbitMqOptions["ResponseQueue"];
            x.AddConsumer<PaymentResponseConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {

                cfg.Host(new Uri($"rabbitmq://{host}:{port}"), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                cfg.ReceiveEndpoint(responseQueue, c =>
                {
                    c.ConfigureConsumer<PaymentResponseConsumer>(context);
                });
            });
        });


        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Trying to update database");
            try
            {
                var context = services.GetRequiredService<AuthorizationDbContext>();
                context.Database.Migrate();
                logger.LogInformation("---> Database updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
            }

        }
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}

