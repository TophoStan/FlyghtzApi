using DataExtracting.Contracts;
using DataExtractor;
using DataExtractor.Wikipedia;
using FastEndpoints;
using FastEndpoints.Swagger;
using FlightsApi.Endpoints;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<TableScraper>();
builder.Services.AddScoped<ListScraper>();
builder.Services.AddScoped<WikepediaAirportExtractor>();

builder.Services.AddScoped<IDataExtractionController, DataExtractionController>();

builder.Services.AddHttpClient();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(x =>
{
    x.ShortSchemaNames = true;
});

builder.Services.AddCors(x =>
{
    x.AddPolicy(name: "local", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("local");
}


app.UseFastEndpoints(x =>
{
    x.Endpoints.ShortNames = true;
}).UseSwaggerGen();


app.UseHttpsRedirection();

app.Run();