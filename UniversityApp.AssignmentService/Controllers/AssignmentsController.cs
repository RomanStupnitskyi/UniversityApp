using Microsoft.AspNetCore.Mvc;
using UniversityApp.AssignmentService.Services;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.AssignmentService.Controllers;

[ApiController]
[Route("/assignments")]
public class AssignmentsController(IAssignmentService assignmentService) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult> GetAllAsync()
	{
		var result = await assignmentService.GetAllAsync();
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpGet("{id:guid}")]
	public async Task<ActionResult> GetByIdAsync(Guid id)
	{
		var result = await assignmentService.GetByIdAsync(id);
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpPost]
	public async Task<ActionResult> CreateAsync([FromBody] CreateAssignmentDto dto)
	{
		var result = await assignmentService.CreateAsync(dto);
		return result.IsSuccess
			? Ok(result.Value)
			: BadRequest(result.Error);
	}
	
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] UpdateAssignmentDto dto)
	{
		var result = await assignmentService.UpdateAsync(id, dto);
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> DeleteAsync(Guid id)
	{
		var result = await assignmentService.DeleteAsync(id);
		return result.IsSuccess
			? NoContent()
			: NotFound(result.Error);
	}
}