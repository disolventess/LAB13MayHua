namespace Lab13_JoseTincopa.Application.DTOs;

public sealed class OrderReportRow
{
    public int OrderId { get; set; }

    public string ClientName { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public int TotalUnits { get; set; }

    public decimal TotalAmount { get; set; }
}