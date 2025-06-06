﻿using Refit;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.API;

public interface ICourseAPI
{
	[Get("/courses/{id}")]
	Task<HttpResponseMessage> GetCourseByIdAsync(Guid id);
}