using Carter;
using IztekCafe.Application;
using IztekCafe.Persistance;
using IztekCafe.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
      .AddApplicationServices()
      .AddPersistanceServices(builder.Configuration)
      .AddWebApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapCarter();
app.Run();