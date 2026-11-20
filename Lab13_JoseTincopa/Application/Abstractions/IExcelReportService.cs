using Lab13_JoseTincopa.Application.DTOs;

namespace Lab13_JoseTincopa.Application.Abstractions;

public interface IExcelReportService
{
    ExcelFile CreateOrdersReport(
        IReadOnlyList<OrderReportRow> rows);

    ExcelFile CreateProductsReport(
        IReadOnlyList<ProductReportRow> rows);
}