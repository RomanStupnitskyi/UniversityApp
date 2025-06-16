namespace UniversityApp.Shared.DTOs;

public class CreateSubmissionDto
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public string Content { get; set; }
}