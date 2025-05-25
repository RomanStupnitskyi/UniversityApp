using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Shared.Models;

public class Course
{
	[Key, Length(36, 36)]
	public string Id { get; init; } = Guid.NewGuid().ToString();

	[Required, MaxLength(100)]
	public string Title { get; set; }

	[MaxLength(500)]
	public string? Description { get; set; }

	[Range(1, 30)]
	public int ECTS { get; set; }

	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}