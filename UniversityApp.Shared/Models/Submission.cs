using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Shared.Models;

public class Submission
{
	[Key, Length(36, 36)]
	public string Id { get; init; } = Guid.NewGuid().ToString();
	
	[Required, Length(36, 36)]
	public string AssignmentId { get; init; }
	public Assignment Assignment { get; init; }
	
	[Required, Length(36, 36)]
	public string StudentId { get; init; }
	
	[Required, MaxLength(500)]
	public string Content { get; set; }
	
	[DataType(DataType.DateTime)]
	public DateTime SubmittedAt { get; init; } = DateTime.UtcNow;
	
	[DataType(DataType.DateTime)]
	public DateTime? LastUpdatedAt { get; set; }
}