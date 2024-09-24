using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Core.Enities.ProjectAggregate;

public class User: IdentityUser
{
        public string FirstName { get; set; }
        public string LastName { get;  set; }
        public string Email { get;  set; }
        public virtual UserRole Role { get;  set; }

        public virtual List<Project> ManagedProjects { get;  set; } = new List<Project>();
        public virtual List<ProjectTask> AssignedTasks  { get;  set; } = new List<ProjectTask>();
        
        public User(){}
    }