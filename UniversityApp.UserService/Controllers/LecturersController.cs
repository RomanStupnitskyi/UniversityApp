using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.UserService.Services;

namespace UniversityApp.UserService.Controllers;

[ApiController]
[Route("/lecturers")]
public class LecturersController(ILecturerService lecturerService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		return await lecturerService.GetAllAsync();
	}
	
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(string id)
	{
		return await lecturerService.GetByIdAsync(id);
	}
	
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateLecturerDto dto)
	{
		return await lecturerService.CreateAsync(dto);
	}
	
	[HttpPut("{id}")]
	public async Task<IActionResult> Update(string id, [FromBody] UpdateLecturerDto dto)
	{
		return await lecturerService.UpdateAsync(id, dto);
	}
	
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		return await lecturerService.DeleteAsync(id);
	}
}