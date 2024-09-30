using API.RequestDTO.Report;
using Core.DTO.ResponseDTO.Report;
using Core.Interfaces;
using Core.Enities.ProjectAggregate;
using Core.Specifications.Report;

namespace Core.Services
{
    public class ReportService : IReportService
    {
        private readonly IAsyncRepository<Project> _projectRepository;
        private readonly IAsyncRepository<ProjectTask> _projectTaskRepository;
        private readonly IAsyncRepository<User> _userRepository;

        public ReportService(IAsyncRepository<User> userRepository, IAsyncRepository<Project> projectRepository,
            IAsyncRepository<ProjectTask> projectTaskRepository)
        {
            _projectRepository = projectRepository;
            _projectTaskRepository = projectTaskRepository;
            _userRepository = userRepository;
        }

        // Chart 1: Task Status Data
        public async Task<ChartDataDto> GetTaskStatusData(ReportFilterDto filter)
        {
            var spec = new TaskFilterSpecification(filter);
            var tasks = await _projectTaskRepository.ListAsync(spec);

            var result = tasks
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            return new ChartDataDto
            {
                Labels = result.Select(r => r.Status.ToString()).ToArray(),
                Data = result.Select(r => r.Count).ToArray()
            };
        }


        // Chart 2: Project Completion Trend Data
        public async Task<ChartDataDto> GetCompletionTrendData(ReportFilterDto filter)
        {
            var spec = new ProjectCompletionTrendSpecification(filter);
            var projects = await _projectRepository.ListAsync(spec);

            var result = projects
                .GroupBy(p => new { p.StartDate.Month, p.StartDate.Year }) // Group by month and year
                .Select(g => new { Month = g.Key.Month, Year = g.Key.Year, Count = g.Count() })
                .ToList();

            // Flatten the results to create labels in "MM/YYYY" format
            return new ChartDataDto
            {
                Labels = result.Select(r => $"{r.Month}/{r.Year}").ToArray(),
                Data = result.Select(r => r.Count).ToArray()
            };
        }


        // Chart 3: Task Breakdown by Translator
        public async Task<ChartDataDto> GetTranslatorBreakdownData(ReportFilterDto filter)
        {
            var spec = new TaskFilterSpecification(filter);
            var tasks = await _projectTaskRepository.ListAsync(spec);
          
            var tasksPerTranslator = tasks
                .GroupBy(x => x.AssignedTranslator.FirstName)
                .Select((g, i) => new { Id = g.Key, Count = g.Count() });
            
            return new ChartDataDto
            {
                Labels = tasksPerTranslator.Select(x => x.Id).ToArray(),
                Data = tasksPerTranslator.Select(x=>x.Count).ToArray()
            };
        }
    }
}