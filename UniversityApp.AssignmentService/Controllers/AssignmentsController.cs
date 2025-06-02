using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.AssignmentService.Services;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Events;
using UniversityApp.Shared.Queries;

namespace UniversityApp.AssignmentService.Controllers;

[ApiController]
[Route("/assignments")]
[Authorize]
public class AssignmentsController(
		IAssignmentService assignmentService,
		IPublishEndpoint publishEndpoint
	) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult> GetAllAsync([FromQuery] AssignmentQuery query)
	{
		var result = await assignmentService.GetAllAsync(query);
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

		await publishEndpoint.Publish(new AssignmentDeletedEvent
		{
			AssignmentId = id
		});
		
		return result.IsSuccess
			? NoContent()
			: NotFound(result.Error);
	}
}