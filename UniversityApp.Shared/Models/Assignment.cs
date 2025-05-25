using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Shared.Models;

public class Assignment
{
	[Key]
	public Guid Id { get; init; } = Guid.NewGuid();
	
	[Required, Length(36, 36)]
	public Guid CourseId { get; init; }

	[Required, MaxLength(100)]
	public string Title { get; set; }

	[MaxLength(500)]
	public string? Description { get; set; }
	
	[DataType(DataType.DateTime)]
	public DateTime? StartDate { get; set; }
	
	[DataType(DataType.DateTime)]
	public DateTime? EndDate { get; set; }
	
	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}