using Lab13_JoseTincopa.Application.Abstractions;
using Lab13_JoseTincopa.Application.DTOs;

namespace Lab13_JoseTincopa.Application.Reports;

public sealed class GetProductsReportHandler
    : IQueryHandler<GetProductsReportQuery, ExcelFile>
{
    private readonly IReportRepository _repository;
    private readonly IExcelReportService _excelService;

    public GetProductsReportHandler(
        IReportRepository repository,
        IExcelReportService excelService)
    {
        _repository = repository;
        _excelService = excelService;
    }

    public async Task<ExcelFile> HandleAsync(
        GetProductsReportQuery query,
        CancellationToken cancellationToken = default)
    {
        var rows = await _repository.GetProductsAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _excelService.CreateProductsReport(rows);
    }
}