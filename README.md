# Backend System

## Overview
This backend system is designed for managing projects, tasks, and reports using Domain-Driven Design (DDD) principles and xUnit for testing. The system ensures clear roles and responsibilities among users while promoting maintainability and scalability.

## Features
- **User Roles**:
  - **Managers**: Can add projects, assign tasks to different translators, and access the reporting module.
  - **Translators**: Can view their assigned tasks, mark them as completed, but cannot delete tasks or create new ones.

- **Projects Management**: Managers can create and oversee projects, assigning tasks to translators. 
- **Tasks Management**: Tasks have three statuses: In Progress, Completed, and Overdue. 
  - All tasks or projects at the start will be considered as **In Progress**.
  - When all tasks within a project are marked as completed, the project is automatically updated to **Completed**.
  
- **Reports Module**: Managers can generate reports for insights and progress tracking.

## Architecture
The system is structured around Domain-Driven Design (DDD), which includes:
- **Bounded Contexts**: Each module (Projects, Tasks, Reports) represents a distinct bounded context.
- **Entities and Value Objects**: Clear separation between entities and value objects to enforce business rules.
- **Aggregates**: Use of aggregates to manage invariants and transactional consistency.

### Testing
The system uses **xUnit** for unit and integration testing to ensure reliability and performance.

## Considerations
While implementing DDD for this system, there are many areas where DDD practices were enforced. This may seem like overkill for the simple business requirements at hand, but it helps in:
- **Maintainability**: A well-structured domain model can make future changes easier.
- **Scalability**: Prepared for future enhancements and complexities.
- **Collaboration**: Provides a clear language for discussing domain concepts with stakeholders.
