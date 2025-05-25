using Microsoft.AspNetCore.Mvc;
using UniversityApp.AssignmentService.Services;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.AssignmentService.Controllers;

[ApiController]
[Route("/")]
public class AssignmentsController(IAssignmentService assignmentService) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult> GetAllAsync()
	{
		var result = await assignmentService.GetAllAsync();
		return result;
	}
	
	[HttpGet("{id}")]
	public async Task<ActionResult> GetByIdAsync(string id)
	{
		var result = await assignmentService.GetByIdAsync(id);
		return result;
	}
	
	[HttpPost]
	public async Task<ActionResult> CreateAsync([FromBody] CreateAssignmentDto dto)
	{
		var result = await assignmentService.CreateAsync(dto);
		return result;
	}
	
	[HttpPut("{id}")]
	public async Task<ActionResult> UpdateAsync(string id, [FromBody] UpdateAssignmentDto dto)
	{
		var result = await assignmentService.UpdateAsync(id, dto);
		return result;
	}
	
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(string id)
	{
		var result = await assignmentService.DeleteAsync(id);
		return result;
	}
}