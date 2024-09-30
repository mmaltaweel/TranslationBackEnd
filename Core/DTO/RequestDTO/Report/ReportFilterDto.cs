namespace API.RequestDTO.Report;

public class ReportFilterDto
{
    public int? ProjectId { get; set; }
    public string? TranslatorId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? DueDate { get; set; }
}
