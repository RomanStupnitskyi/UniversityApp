using MassTransit;
using UniversityApp.AssignmentService.Repositories;
using UniversityApp.Shared.Events;

namespace UniversityApp.AssignmentService.Consumers.Assignments;

public class AssignmentsDeletedConsumer(ISubmissionRepository submissionRepository) : IConsumer<AssignmentsDeletedEvent>
{
	public async Task Consume(ConsumeContext<AssignmentsDeletedEvent> context)
	{
		
	}
}