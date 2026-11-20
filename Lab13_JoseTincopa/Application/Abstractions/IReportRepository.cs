using Lab13_JoseTincopa.Application.DTOs;

namespace Lab13_JoseTincopa.Application.Abstractions;

public interface IReportRepository
{
    Task<IReadOnlyList<OrderReportRow>> GetOrdersAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductReportRow>> GetProductsAsync(
        CancellationToken cancellationToken = default);
}