namespace UniversityApp.Shared.DTOs;

public class CreateAssignmentDto
{
	public string Id { get; init; } = Guid.NewGuid().ToString();
	public string CourseId { get; set; }
	public string Title { get; set; }
	public string? Description { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
}