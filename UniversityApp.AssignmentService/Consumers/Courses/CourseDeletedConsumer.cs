using MassTransit;
using UniversityApp.AssignmentService.Services;
using UniversityApp.Shared.Events;

namespace UniversityApp.AssignmentService.Consumers.Courses;

public sealed class CourseDeletedConsumer(IAssignmentService assignmentService) : IConsumer<CourseDeletedEvent>
{
	public async Task Consume(ConsumeContext<CourseDeletedEvent> context)
	{
		var course = new CourseDeletedEvent
		{
			CourseId = context.Message.CourseId,
			DeletedAt = context.Message.DeletedAt
		};
		
		await assignmentService.DeleteByCourseIdAsync(course.CourseId);
	}
}