using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;
using UniversityApp.UserService.Services;

namespace UniversityApp.UserService.Controllers;

[ApiController]
[Route("/lecturers")]
public class LecturersController(ILecturerService lecturerService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var result = await lecturerService.GetAllAsync();
		return result.IsSuccess
			? Ok(result.Value)
			: Conflict(result.Error);
	}
	
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var result = await lecturerService.GetByIdAsync(id);
		return result.IsSuccess
			? Ok(result.Value)
			: Conflict(result.Error);
	}
	
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateLecturerDto dto)
	{
		var result = await lecturerService.CreateAsync(dto);
		return result.IsSuccess
			? Ok(result.Value)
			: Conflict(result.Error);
	}
	
	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLecturerDto dto)
	{
		var result = await lecturerService.UpdateAsync(id, dto);
		return result.IsSuccess
			? Ok(result.Value)
			: Conflict(result.Error);
	}
	
	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var result = await lecturerService.DeleteAsync(id);
		return result.IsSuccess
			? NoContent()
			: Conflict(result.Error);
	}
}