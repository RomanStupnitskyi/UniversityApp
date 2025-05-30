using MassTransit;
using UniversityApp.AssignmentService.Services;
using UniversityApp.Shared.Events;

namespace UniversityApp.AssignmentService.Consumers.Assignments;

public class AssignmentsDeletedConsumer(ISubmissionService submissionService) : IConsumer<AssignmentsDeletedEvent>
{
	public async Task Consume(ConsumeContext<AssignmentsDeletedEvent> context)
	{
		var assignments = new AssignmentsDeletedEvent
		{
			Assignments = context.Message.Assignments,
			DeletedAt = context.Message.DeletedAt
		};

		var assignmentIds = assignments.Assignments.Select(a => a.Id).ToList();
		await submissionService.DeleteByAssignmentIdsAsync(assignmentIds);
	}
}