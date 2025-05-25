using MassTransit;
using UniversityApp.AssignmentService.Repositories;
using UniversityApp.Shared.Events;

namespace UniversityApp.AssignmentService.Consumers.Assignments;

public sealed class AssignmentDeletedConsumer(
	ISubmissionRepository submissionRepository,
	IPublishEndpoint publishEndpoint
	) : IConsumer<AssignmentDeletedEvent>
{
	public async Task Consume(ConsumeContext<AssignmentDeletedEvent> context)
	{
		var assignment = new AssignmentDeletedEvent
		{
			AssignmentId = context.Message.AssignmentId,
			DeletedAt = context.Message.DeletedAt
		};

		await submissionRepository.DeleteByAssignmentIdAsync(assignment.AssignmentId);
	}
}