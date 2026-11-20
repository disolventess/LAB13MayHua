using System.Reflection;
using Lab13_JoseTincopa.Application.Abstractions;
using Lab13_JoseTincopa.Application.DTOs;
using Lab13_JoseTincopa.Application.Reports;
using Lab13_JoseTincopa.Infrastructure.Data;
using Lab13_JoseTincopa.Infrastructure.Excel;
using Lab13_JoseTincopa.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// API REST
builder.Services.AddControllers();

// Respuestas estandarizadas para errores
builder.Services.AddProblemDetails();

// Swagger y documentación XML
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
    var xmlPath = Path.Combine(
        AppContext.BaseDirectory,
        $"{assemblyName}.xml");

    options.IncludeXmlComments(xmlPath);
});

// Infraestructura
builder.Services.AddSingleton<SqlConnectionFactory>();

builder.Services.AddScoped<
    IReportRepository,
    ReportRepository>();

builder.Services.AddScoped<
    IExcelReportService,
    ExcelReportService>();

// CQRS: registro de handlers de consulta
builder.Services.AddScoped<
    IQueryHandler<GetOrdersReportQuery, ExcelFile>,
    GetOrdersReportHandler>();

builder.Services.AddScoped<
    IQueryHandler<GetProductsReportQuery, ExcelFile>,
    GetProductsReportHandler>();

var app = builder.Build();

// Manejo global de excepciones
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();