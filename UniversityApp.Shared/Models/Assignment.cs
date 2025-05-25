using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Shared.Models;

public class Assignment
{
	[Key, Length(36, 36)]
	public string Id { get; init; } = Guid.NewGuid().ToString();
	
	[Required, Length(36, 36)]
	public string CourseId { get; init; }

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