using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Shared.Models;

public class Course
{
	[Key]
	public Guid Id { get; init; } = Guid.NewGuid();

	[Required, MaxLength(100)]
	public string Title { get; set; }

	[MaxLength(500)]
	public string? Description { get; set; }

	[Range(1, 30)]
	public int ECTS { get; set; }

	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}