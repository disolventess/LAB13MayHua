using ClosedXML.Excel;
using Lab13_JoseTincopa.Application.Abstractions;
using Lab13_JoseTincopa.Application.DTOs;

namespace Lab13_JoseTincopa.Infrastructure.Excel;

public sealed class ExcelReportService : IExcelReportService
{
    public ExcelFile CreateOrdersReport(
        IReadOnlyList<OrderReportRow> rows)
    {
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Pedidos");

        PrepareHeader(
            worksheet,
            "REPORTE DE PEDIDOS",
            new[]
            {
                "Pedido",
                "Cliente",
                "Fecha",
                "Unidades",
                "Importe"
            });

        int rowNumber = 4;

        foreach (var row in rows)
        {
            worksheet.Cell(rowNumber, 1).Value = row.OrderId;
            worksheet.Cell(rowNumber, 2).Value = row.ClientName;
            worksheet.Cell(rowNumber, 3).Value = row.OrderDate;
            worksheet.Cell(rowNumber, 4).Value = row.TotalUnits;
            worksheet.Cell(rowNumber, 5).Value = row.TotalAmount;

            rowNumber++;
        }

        if (rows.Count > 0)
        {
            worksheet.Range(4, 3, rowNumber - 1, 3)
                .Style.DateFormat.Format = "dd/MM/yyyy";

            worksheet.Range(4, 5, rowNumber - 1, 5)
                .Style.NumberFormat.Format = "#,##0.00";
        }

        int totalRow = rowNumber + 1;

        worksheet.Cell(totalRow, 3).Value = "TOTAL";
        worksheet.Cell(totalRow, 4).Value =
            rows.Sum(row => row.TotalUnits);
        worksheet.Cell(totalRow, 5).Value =
            rows.Sum(row => row.TotalAmount);

        FormatTotals(worksheet, totalRow);
        FinishSheet(worksheet, rows.Count, "TablaPedidos");

        return SaveWorkbook(workbook, "Reporte_Pedidos.xlsx");
    }

    public ExcelFile CreateProductsReport(
        IReadOnlyList<ProductReportRow> rows)
    {
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Productos");

        PrepareHeader(
            worksheet,
            "REPORTE DE VENTAS POR PRODUCTO",
            new[]
            {
                "ID",
                "Producto",
                "Precio actual",
                "Unidades vendidas",
                "Importe"
            });

        int rowNumber = 4;

        foreach (var row in rows)
        {
            worksheet.Cell(rowNumber, 1).Value = row.ProductId;
            worksheet.Cell(rowNumber, 2).Value = row.ProductName;
            worksheet.Cell(rowNumber, 3).Value = row.CurrentPrice;
            worksheet.Cell(rowNumber, 4).Value = row.UnitsSold;
            worksheet.Cell(rowNumber, 5).Value = row.TotalAmount;

            rowNumber++;
        }

        if (rows.Count > 0)
        {
            worksheet.Range(4, 3, rowNumber - 1, 3)
                .Style.NumberFormat.Format = "#,##0.00";

            worksheet.Range(4, 5, rowNumber - 1, 5)
                .Style.NumberFormat.Format = "#,##0.00";
        }

        int totalRow = rowNumber + 1;

        worksheet.Cell(totalRow, 3).Value = "TOTAL";
        worksheet.Cell(totalRow, 4).Value =
            rows.Sum(row => row.UnitsSold);
        worksheet.Cell(totalRow, 5).Value =
            rows.Sum(row => row.TotalAmount);

        FormatTotals(worksheet, totalRow);
        FinishSheet(worksheet, rows.Count, "TablaProductos");

        return SaveWorkbook(workbook, "Reporte_Productos.xlsx");
    }

    private static void PrepareHeader(
        IXLWorksheet worksheet,
        string title,
        string[] headers)
    {
        worksheet.Range("A1:E1").Merge();
        worksheet.Cell("A1").Value = title;

        var titleRange = worksheet.Range("A1:E1");
        titleRange.Style.Font.Bold = true;
        titleRange.Style.Font.FontSize = 16;
        titleRange.Style.Font.FontColor = XLColor.White;
        titleRange.Style.Fill.BackgroundColor = XLColor.DarkBlue;

        worksheet.Row(1).Height = 30;

        worksheet.Range("A2:E2").Merge();
        worksheet.Cell("A2").Value =
            "Base: LINQExample | Importes calculados con precios actuales";
        worksheet.Cell("A2").Style.Font.Italic = true;

        for (int index = 0; index < headers.Length; index++)
        {
            worksheet.Cell(3, index + 1).Value = headers[index];
        }

        var headerRange = worksheet.Range("A3:E3");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
    }

    private static void FormatTotals(
        IXLWorksheet worksheet,
        int totalRow)
    {
        var totals = worksheet.Range(totalRow, 1, totalRow, 5);

        totals.Style.Font.Bold = true;
        totals.Style.Fill.BackgroundColor = XLColor.LightGray;

        worksheet.Cell(totalRow, 5)
            .Style.NumberFormat.Format = "#,##0.00";
    }

    private static void FinishSheet(
        IXLWorksheet worksheet,
        int count,
        string tableName)
    {
        if (count > 0)
        {
            var table = worksheet
                .Range(3, 1, count + 3, 5)
                .CreateTable(tableName);

            table.Theme = XLTableTheme.TableStyleMedium2;
        }

        worksheet.SheetView.FreezeRows(3);
        worksheet.Columns(1, 5).AdjustToContents();

        worksheet.Column(2).Width = 28;
        worksheet.Column(3).Width = 20;
        worksheet.Column(4).Width = 22;
        worksheet.Column(5).Width = 18;
    }

    private static ExcelFile SaveWorkbook(
        XLWorkbook workbook,
        string fileName)
    {
        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return new ExcelFile(stream.ToArray(), fileName);
    }
}