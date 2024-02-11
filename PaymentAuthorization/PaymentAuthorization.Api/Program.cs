using PaymentAuthorization.Api.AsyncDataServices;
using PaymentAuthorization.Api.Data.Repository;
using PaymentAuthorization.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPaymentAuthorizationService, PaymentAuthorizationService>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddSingleton<IPaymentAuthorizationRepository, PaymentAuthorizationRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
