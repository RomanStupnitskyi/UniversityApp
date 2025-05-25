namespace UniversityApp.Shared.DTOs;

public class CreateSubmissionDto
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public Guid StudentId { get; init; }
	public string Content { get; set; }
}