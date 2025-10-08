using Microsoft.Extensions.DependencyInjection;
using LocOn.Context;
using Microsoft.EntityFrameworkCore;
using LocOn.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BdContext>(options =>
{
    options.UseMySql(connectionString, 
        ServerVersion.AutoDetect(connectionString));
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<FilmeService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<PlanoService>();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<StripePaymentService>();

Stripe.StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeSettings:SecretKey");

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
