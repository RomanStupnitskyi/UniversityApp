using MassTransit;
using UniversityApp.AssignmentService.Repositories;
using UniversityApp.AssignmentService.Services;
using UniversityApp.Shared.Events;

namespace UniversityApp.AssignmentService.Consumers.Assignments;

public sealed class AssignmentDeletedConsumer(ISubmissionService submissionService)
	: IConsumer<AssignmentDeletedEvent>
{
	public async Task Consume(ConsumeContext<AssignmentDeletedEvent> context)
	{
		var assignment = new AssignmentDeletedEvent
		{
			AssignmentId = context.Message.AssignmentId,
			DeletedAt = context.Message.DeletedAt
		};

		await submissionService.DeleteByAssignmentIdAsync(assignment.AssignmentId);
	}
}