using API.RequestDTO.Report;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Translator, ProjectManager")]
    public class ReportController : Controller
    {   
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        
        // Endpoint to get Task Status Data
        [HttpGet("task-status")]
        public async Task<IActionResult> GetTaskStatusData([FromQuery] ReportFilterDto filter)
        {
            var result = await _reportService.GetTaskStatusData(filter);
            return Ok(result);
        }

        // Endpoint to get Project Completion Trend Data
        [HttpGet("completion-trend")]
        public async Task<IActionResult> GetCompletionTrendData([FromQuery] ReportFilterDto filter)
        {
            var result = await _reportService.GetCompletionTrendData(filter);
            return Ok(result);
        }

        // Endpoint to get Translator Breakdown Data
        [HttpGet("translator-breakdown")]
        public async Task<IActionResult> GetTranslatorBreakdownData([FromQuery] ReportFilterDto filter)
        {
            var result = await _reportService.GetTranslatorBreakdownData(filter);
            return Ok(result);
        }
    }
}