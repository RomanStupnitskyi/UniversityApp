using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Shared.Models;

public class Submission
{
	[Key]
	public Guid Id { get; init; } = Guid.NewGuid();
	
	[Required, Length(36, 36)]
	public Guid AssignmentId { get; init; }
	
	[Required, Length(36, 36)]
	public Guid StudentId { get; init; }
	
	[Required, MaxLength(500)]
	public string Content { get; set; }
	
	[DataType(DataType.DateTime)]
	public DateTime SubmittedAt { get; init; } = DateTime.UtcNow;
	
	[DataType(DataType.DateTime)]
	public DateTime? LastUpdatedAt { get; set; }
}