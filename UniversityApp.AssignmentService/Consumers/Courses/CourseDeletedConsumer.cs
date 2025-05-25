using MassTransit;
using UniversityApp.AssignmentService.Repositories;
using UniversityApp.Shared.Events;

namespace UniversityApp.AssignmentService.Consumers.Courses;

public sealed class CourseDeletedConsumer(IAssignmentRepository assignmentRepository) : IConsumer<CourseDeletedEvent>
{
	public async Task Consume(ConsumeContext<CourseDeletedEvent> context)
	{
		var course = new CourseDeletedEvent
		{
			CourseId = context.Message.CourseId,
			DeletedAt = context.Message.DeletedAt
		};
		
		await assignmentRepository.DeleteByCourseIdAsync(course.CourseId);
	}
}