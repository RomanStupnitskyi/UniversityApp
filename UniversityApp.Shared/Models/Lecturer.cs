using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Shared.Models;

public class Lecturer
{
	[Key]
	public Guid Id { get; init; }

	[Length(9, 11)]
	public string? ContactNumber { get; set; }

	[EmailAddress]
	public string? ContactEmail { get; set; }
	
	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}