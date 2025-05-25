using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Shared.Models;

public class Student
{
	[Key]
	public Guid Id { get; init; }

	[Required, Length(6, 12)]
	public string StudentNumber { get; set; }

	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}