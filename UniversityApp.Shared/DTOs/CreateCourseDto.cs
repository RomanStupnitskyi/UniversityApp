namespace UniversityApp.Shared.DTOs;

public class CreateCourseDto
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public string Title { get; set; }
	public string? Description { get; set; }
	public int ECTS { get; set; }
}