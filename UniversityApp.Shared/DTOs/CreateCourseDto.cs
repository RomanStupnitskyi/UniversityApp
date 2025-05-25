namespace UniversityApp.Shared.DTOs;

public class CreateCourseDto
{
	public string Id { get; init; } = Guid.NewGuid().ToString();
	public string Title { get; set; }
	public string? Description { get; set; }
	public int ECTS { get; set; }
}