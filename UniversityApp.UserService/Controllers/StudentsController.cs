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
		return await studentService.GetAllAsync();
	}
	
	[HttpGet("{id}")]
	public async Task<ActionResult> GetById(string id)
	{
		return await studentService.GetByIdAsync(id);
	}
	
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateStudentDto dto)
	{
		return Ok(await studentService.CreateAsync(dto));
	}
	
	[HttpPut("{id}")]
	public async Task<ActionResult> Update(string id, [FromBody] UpdateStudentDto dto)
	{
		return await studentService.UpdateAsync(id, dto);
	}
	
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(string id)
	{
		return await studentService.DeleteAsync(id);
	}
}