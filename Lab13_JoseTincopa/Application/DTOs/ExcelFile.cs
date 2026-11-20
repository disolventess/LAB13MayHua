namespace Lab13_JoseTincopa.Application.DTOs;

public sealed record ExcelFile(byte[] Content, string FileName)
{
    public const string ContentType =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
}