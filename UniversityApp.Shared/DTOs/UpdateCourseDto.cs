namespace UniversityApp.Shared.DTOs;

public class UpdateCourseDto
{
	public string? Title { get; set; }
	public string? Description { get; set; }
	public int? ECTS { get; set; }
}