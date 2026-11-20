namespace Lab13_JoseTincopa.Application.DTOs;

public sealed class ProductReportRow
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal CurrentPrice { get; set; }

    public int UnitsSold { get; set; }

    public decimal TotalAmount { get; set; }
}