namespace UniversityApp.Shared.DTOs;

public class CreateSubmissionDto
{
	public string Id { get; init; } = Guid.NewGuid().ToString();
	public string StudentId { get; init; }
	public string Content { get; set; }
}