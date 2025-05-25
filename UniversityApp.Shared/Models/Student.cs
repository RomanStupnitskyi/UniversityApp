using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Shared.Models;

public class Student
{
	[Key, Length(36, 36)]
	public string Id { get; init; }

	[Required, Length(6, 12)]
	public string StudentNumber { get; set; }

	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}