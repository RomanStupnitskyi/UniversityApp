namespace UniversityApp.Shared.Events;

public class CourseDeletedEvent
{
	public Guid CourseId { get; set; }
	public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
}