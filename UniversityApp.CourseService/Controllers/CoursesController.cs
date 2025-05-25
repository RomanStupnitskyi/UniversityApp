using Microsoft.AspNetCore.Mvc;
using UniversityApp.CourseService.Services;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.CourseService.Controllers;

[ApiController]
[Route("/")]
public class CoursesController(ICourseService courseService) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult> GetAllAsync()
	{
		var result = await courseService.GetAllAsync();
		return result;
	}
	
	[HttpGet("{id}")]
	public async Task<ActionResult> GetByIdAsync(string id)
	{
		var result = await courseService.GetByIdAsync(id);
		return result;
	}
	
	[HttpPost]
	public async Task<ActionResult> CreateAsync([FromBody] CreateCourseDto dto)
	{
		var result = await courseService.CreateAsync(dto);
		return result;
	}
	
	[HttpPut("{id}")]
	public async Task<ActionResult> UpdateAsync(string id, [FromBody] UpdateCourseDto dto)
	{
		var result = await courseService.UpdateAsync(id, dto);
		return result;
	}
	
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(string id)
	{
		var result = await courseService.DeleteAsync(id);
		return result;
	}
}