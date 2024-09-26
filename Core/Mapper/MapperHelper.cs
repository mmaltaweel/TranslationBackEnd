using Core.DTO.ResponseDTO;
using Core.Enities.ProjectAggregate;

namespace Core.Mapper;

public static class MapperHelper
{
    public static ProjectResponse ToProjectResponse(this Project project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            StatusDisplayName = project.Status.ToString(),
            Tasks = project.Tasks.Select(t => t.ToTaskResponse()).ToList()
        };
    }

    public static TaskResponse ToTaskResponse(this ProjectTask task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Status = task.Status,
            StatusDisplayName = task.Status.ToString(),
            Description = task.Description,
            DueDate = task.DueDate,
            AssignedTranslatorId = task.AssignedTranslatorId
        };
    }
}