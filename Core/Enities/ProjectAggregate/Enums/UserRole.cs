using System.ComponentModel;

namespace Core.Enities.ProjectAggregate;

public enum UserRole
{
    [Description("ProjectManager")]
    ProjectManager,
    [Description("Translator")]
    Translator
}