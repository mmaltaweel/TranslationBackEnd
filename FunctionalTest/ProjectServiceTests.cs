using Core.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Net;
using System.Threading.Tasks;
using API.RequestDTO;
using Core.Enities.ProjectAggregate;
using Core.Services;
using Xunit;
namespace FunctionalTest;

public class ProjectServiceTests
{
    private readonly Mock<IAsyncRepository<Project>> _projectRepositoryMock;
    private readonly Mock<IAsyncRepository<User>> _userRepositoryMock;
    private readonly Mock<ILog> _logMock;
    private readonly ProjectService _projectService;
    public ProjectServiceTests()
    {
        _projectRepositoryMock = new Mock<IAsyncRepository<Project>>();
        _userRepositoryMock = new Mock<IAsyncRepository<User>>();
        _logMock = new Mock<ILog>();
        _projectService = new ProjectService(_projectRepositoryMock.Object, _userRepositoryMock.Object, _logMock.Object);
    }
    
    [Fact]
    public async Task CreateProject_ShouldReturnCreatedResult_WhenProjectIsSuccessfullyCreated()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("id", "2450415e-4991-4f4d-b781-39356d1633d9"), 
            new Claim("name", "test"),
            new Claim("NameIdentifier","2450415e-4991-4f4d-b781-39356d1633d9")
        }));
    
        var requestDto = new ProjectRequest 
        { 
            Name = "Test Project", 
            Description = "Test Description", 
            StartDate = DateTime.Now, 
            EndDate = DateTime.Now.AddDays(30)
        };
    
        // Create a mock project manager user
        var projectManager = new User { Id = "2450415e-4991-4f4d-b781-39356d1633d9", UserName = "Manager Name", Role = UserRole.ProjectManager };

        // Setup the user repository to return the project manager when the user ID is requested
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync("2450415e-4991-4f4d-b781-39356d1633d9")).ReturnsAsync(projectManager);
    
        // Setup the project repository to complete successfully
        var newProject = new Project(); // Create an instance of Project or use a predefined one
        _projectRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Project>()))
            .ReturnsAsync(newProject);

        // Act
        var result = await _projectService.CreateProject(requestDto, user);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        _projectRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Once);
    }
    [Fact]
    public async Task CreateProject_ShouldReturnBadRequest_WhenEndDateIsBeforeStartDate()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("id", "1"), new Claim("name", "test") }));

        var requestDto = new ProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description",
            StartDate = DateTime.Now.AddDays(30), // Start date is after the end date
            EndDate = DateTime.Now
        };

        // Act
        var result = await _projectService.CreateProject(requestDto, user);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        _projectRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Never);
    }
    [Fact]
    public async Task CreateProject_ShouldReturnUnauthorized_WhenUserIsNotProjectManager()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("id", "2"), new Claim("name", "test"), new Claim("role", "User") }));

        var requestDto = new ProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30)
        };

        // Act
        var result = await _projectService.CreateProject(requestDto, user);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        _projectRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Never);
    }
    

}