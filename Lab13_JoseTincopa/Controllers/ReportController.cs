using Lab13_JoseTincopa.Application.Abstractions;
using Lab13_JoseTincopa.Application.DTOs;
using Lab13_JoseTincopa.Application.Reports;
using Microsoft.AspNetCore.Mvc;

namespace Lab13_JoseTincopa.Controllers;

[ApiController]
[Route("api/reportes")]
public sealed class ReportsController : ControllerBase
{
    private readonly
        IQueryHandler<GetOrdersReportQuery, ExcelFile> _ordersHandler;

    private readonly
        IQueryHandler<GetProductsReportQuery, ExcelFile> _productsHandler;

    public ReportsController(
        IQueryHandler<GetOrdersReportQuery, ExcelFile> ordersHandler,
        IQueryHandler<GetProductsReportQuery, ExcelFile> productsHandler)
    {
        _ordersHandler = ordersHandler;
        _productsHandler = productsHandler;
    }

    /// <summary>
    /// Descarga el reporte de pedidos en Excel.
    /// </summary>
    /// <remarks>
    /// Incluye cliente, fecha, unidades e importe de cada pedido.
    /// Los pedidos sin detalles aparecen con total cero.
    /// Los importes se calculan utilizando los precios actuales.
    /// </remarks>
    /// <response code="200">Archivo Excel del reporte de pedidos.</response>
    /// <response code="500">Error interno al generar el reporte.</response>
    [HttpGet("pedidos")]
    [Produces(ExcelFile.ContentType)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status500InternalServerError,
        "application/problem+json")]
    public async Task<IActionResult> GetOrders(
        CancellationToken cancellationToken)
    {
        var file = await _ordersHandler.HandleAsync(
            new GetOrdersReportQuery(),
            cancellationToken);

        return File(file.Content, ExcelFile.ContentType, file.FileName);
    }

    /// <summary>
    /// Descarga el reporte de ventas por producto en Excel.
    /// </summary>
    /// <remarks>
    /// Incluye precio actual, unidades vendidas e importe por producto.
    /// Los productos sin ventas aparecen con total cero.
    /// </remarks>
    /// <response code="200">Archivo Excel de ventas por producto.</response>
    /// <response code="500">Error interno al generar el reporte.</response>
    [HttpGet("productos")]
    [Produces(ExcelFile.ContentType)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status500InternalServerError,
        "application/problem+json")]
    public async Task<IActionResult> GetProducts(
        CancellationToken cancellationToken)
    {
        var file = await _productsHandler.HandleAsync(
            new GetProductsReportQuery(),
            cancellationToken);

        return File(file.Content, ExcelFile.ContentType, file.FileName);
    }
}