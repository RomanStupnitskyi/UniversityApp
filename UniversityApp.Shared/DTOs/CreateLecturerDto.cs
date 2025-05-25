namespace UniversityApp.Shared.DTOs;

public class CreateLecturerDto
{
	public Guid Id { get; set; }
	public string? ContactNumber { get; set; }
	public string? ContactEmail { get; set; }
}