using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Shared.Models;

public class Lecturer
{
	[Key, Length(36, 36)]
	public string Id { get; init; }

	[Length(9, 11)]
	public string? ContactNumber { get; set; }

	[EmailAddress]
	public string? ContactEmail { get; set; }
	
	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}