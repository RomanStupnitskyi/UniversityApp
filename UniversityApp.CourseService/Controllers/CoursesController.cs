using Microsoft.AspNetCore.Mvc;
using UniversityApp.CourseService.Services;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.CourseService.Controllers;

[ApiController]
[Route("/courses")]
public class CoursesController(ICourseService courseService) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult> GetAllAsync()
	{
		var result = await courseService.GetAllAsync();
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpGet("{id:guid}")]
	public async Task<ActionResult> GetByIdAsync(Guid id)
	{
		var result = await courseService.GetByIdAsync(id);
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpPost]
	public async Task<ActionResult> CreateAsync([FromBody] CreateCourseDto dto)
	{
		var result = await courseService.CreateAsync(dto);
		return result.IsSuccess
			? Ok(result.Value)
			: BadRequest(result.Error);
	}
	
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] UpdateCourseDto dto)
	{
		var result = await courseService.UpdateAsync(id, dto);
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> DeleteAsync(Guid id)
	{
		var result = await courseService.DeleteAsync(id);
		return result.IsSuccess
			? NoContent()
			: NotFound(result.Error);
	}
}