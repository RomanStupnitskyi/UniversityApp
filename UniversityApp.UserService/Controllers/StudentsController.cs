using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.UserService.Services;

namespace UniversityApp.UserService.Controllers;

[ApiController]
[Route("/students")]
public class StudentsController(IStudentService studentService) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult> GetAll()
	{
		var result = await studentService.GetAllAsync();
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
	public async Task<ActionResult> Delete(Guid id)
	{
		var result = await studentService.DeleteAsync(id);
		return result.IsSuccess
			? NoContent()
			: NotFound(result.Error);
	}
}