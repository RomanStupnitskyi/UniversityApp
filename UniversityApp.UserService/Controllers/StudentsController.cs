using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Queries;
using UniversityApp.UserService.Services;

namespace UniversityApp.UserService.Controllers;

[ApiController]
[Route("/students")]
[Authorize]
public class StudentsController(IStudentService studentService) : ControllerBase
{
	[HttpGet]
	[Authorize(Roles = "admin")]
	public async Task<ActionResult> GetAll([FromQuery] StudentQuery query)
	{
		var result = await studentService.GetAllAsync(query);
		return result.IsSuccess
			? Ok(result.Value)
			: BadRequest(result.Error);
	}
	
	[HttpGet("{id:guid}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var result = await studentService.GetByIdAsync(id);
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateStudentDto dto)
	{
		var result = await studentService.CreateAsync(dto);
		return result.IsSuccess
			? Ok(result.Value)
			: BadRequest(result.Error);
	}
	
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateStudentDto dto)
	{
		var result = await studentService.UpdateAsync(id, dto);
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpDelete("{id:guid}")]
	[Authorize(Roles = "admin")]
	public async Task<ActionResult> Delete(Guid id)
	{
		var result = await studentService.DeleteAsync(id);
		return result.IsSuccess
			? NoContent()
			: NotFound(result.Error);
	}
}