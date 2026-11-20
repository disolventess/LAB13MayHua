using Lab13_JoseTincopa.Application.Abstractions;
using Lab13_JoseTincopa.Application.DTOs;

namespace Lab13_JoseTincopa.Application.Reports;

public sealed record GetOrdersReportQuery : IQuery<ExcelFile>;