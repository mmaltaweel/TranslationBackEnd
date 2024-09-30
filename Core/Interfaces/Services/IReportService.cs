using API.RequestDTO.Report;
using Core.DTO.ResponseDTO.Report;

namespace Core.Interfaces;

public interface IReportService
{
    Task<ChartDataDto> GetTaskStatusData(ReportFilterDto filter);
    Task<ChartDataDto> GetCompletionTrendData(ReportFilterDto filter);
    Task<ChartDataDto> GetTranslatorBreakdownData(ReportFilterDto filter);
}
