 
using Core.Enities.ProjectAggregate;

namespace Core.DTO.ResponseDTO
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get;  set; }
        public string AssignedTranslatorId { get;  set; }
        public string StatusDisplayName { get; set; } 
        public ProjectStatus Status { get; set; }

        public DateTime DueDate { get;  set; }
        public string ProjectName{ get;  set; }
    }
}