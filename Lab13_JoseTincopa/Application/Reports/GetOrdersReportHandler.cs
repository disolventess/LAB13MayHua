using Lab13_JoseTincopa.Application.Abstractions;
using Lab13_JoseTincopa.Application.DTOs;

namespace Lab13_JoseTincopa.Application.Reports;

public sealed class GetOrdersReportHandler
    : IQueryHandler<GetOrdersReportQuery, ExcelFile>
{
    private readonly IReportRepository _repository;
    private readonly IExcelReportService _excelService;

    public GetOrdersReportHandler(
        IReportRepository repository,
        IExcelReportService excelService)
    {
        _repository = repository;
        _excelService = excelService;
    }

    public async Task<ExcelFile> HandleAsync(
        GetOrdersReportQuery query,
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetOrdersAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _excelService.CreateOrdersReport(rows);
    }
}