using Dapper;
using Lab13_JoseTincopa.Application.Abstractions;
using Lab13_JoseTincopa.Application.DTOs;
using Lab13_JoseTincopa.Infrastructure.Data;

namespace Lab13_JoseTincopa.Infrastructure.Repositories;

public sealed class ReportRepository : IReportRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public ReportRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<OrderReportRow>> GetOrdersAsync(
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                o.OrderId,
                c.Name AS ClientName,
                o.OrderDate,
                COALESCE(SUM(od.Quantity), 0) AS TotalUnits,
                COALESCE(SUM(od.Quantity * p.Price), 0) AS TotalAmount
            FROM dbo.Orders AS o
            INNER JOIN dbo.Clients AS c
                ON c.ClientId = o.ClientId
            LEFT JOIN dbo.OrderDetails AS od
                ON od.OrderId = o.OrderId
            LEFT JOIN dbo.Products AS p
                ON p.ProductId = od.ProductId
            GROUP BY
                o.OrderId,
                c.Name,
                o.OrderDate
            ORDER BY
                o.OrderDate,
                o.OrderId;
            """;

        await using var connection =
            _connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            sql,
            cancellationToken: cancellationToken);

        var rows = await connection.QueryAsync<OrderReportRow>(command);

        return rows.ToList();
    }

    public async Task<IReadOnlyList<ProductReportRow>> GetProductsAsync(
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                p.ProductId,
                p.Name AS ProductName,
                p.Price AS CurrentPrice,
                COALESCE(SUM(od.Quantity), 0) AS UnitsSold,
                COALESCE(SUM(od.Quantity * p.Price), 0) AS TotalAmount
            FROM dbo.Products AS p
            LEFT JOIN dbo.OrderDetails AS od
                ON od.ProductId = p.ProductId
            GROUP BY
                p.ProductId,
                p.Name,
                p.Price
            ORDER BY
                TotalAmount DESC,
                p.ProductId;
            """;

        await using var connection =
            _connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            sql,
            cancellationToken: cancellationToken);

        var rows = await connection.QueryAsync<ProductReportRow>(command);

        return rows.ToList();
    }
}