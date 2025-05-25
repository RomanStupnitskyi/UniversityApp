using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.CourseService.Services;

public interface ICourseService
{
	Task<ActionResult> GetAllAsync();
	Task<ActionResult> GetByIdAsync(string id);
	Task<ActionResult> CreateAsync(CreateCourseDto dto);
	Task<ActionResult> UpdateAsync(string id, UpdateCourseDto dto);
	Task<ActionResult> DeleteAsync(string id);
}