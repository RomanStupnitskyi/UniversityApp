namespace UniversityApp.Shared.Events;

public class AssignmentDeletedEvent
{
	public Guid AssignmentId { get; set; }
	public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
}