using UniversityApp.Shared.Models;

namespace UniversityApp.Shared.Events;

public class AssignmentsDeletedEvent
{
	public List<Assignment> Assignments { get; set; } = [];
	public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
}