using Refit;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.API;

public interface ICourseAPI
{
	[Get("/{id}")]
	Task<HttpResponseMessage> GetCourseByIdAsync(Guid id);
}