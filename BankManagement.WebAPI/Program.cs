using BankManagement.Application;
using BankManagement.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
  config.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "BankManagement.WebAPI v1",
    Version = "v1"
  });
  //   config.SwaggerDoc("v2", new OpenApiInfo
  //   {
  //     Title = "BankManagement.WebAPI v2",
  //     Version = "v2"
  //   });
  //   config.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});
builder.Services.AddCors(options =>
{
  options.AddPolicy("Default", builder =>
  {
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(config =>
  {
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "BankManagement.WebAPI v1");
    // config.SwaggerEndpoint("/swagger/v2/swagger.json", "BankManagement.WebAPI v2");
    config.RoutePrefix = string.Empty;
  });
}

app.UseHttpsRedirection();

app.UseCors("Default");

app.UseAuthorization();

app.MapControllers();

app.Run();
