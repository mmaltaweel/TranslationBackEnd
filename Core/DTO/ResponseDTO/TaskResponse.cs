using Core.Enities.ProjectAggregate;

namespace Core.DTO.ResponseDTO
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get;  set; }
        public string AssignedTranslatorId { get;  set; }
        public TaskEStatus Status { get; set; } 
        public DateTime DueDate { get;  set; }
    }
}