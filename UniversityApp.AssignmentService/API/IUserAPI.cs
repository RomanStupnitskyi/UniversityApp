using Refit;

namespace UniversityApp.AssignmentService.API;

public interface IUserAPI
{
	[Get("/students/{id}")]
	Task<HttpResponseMessage> GetStudentByIdAsync(Guid id);
}