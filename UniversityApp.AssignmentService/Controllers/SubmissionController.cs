using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.AssignmentService.Services;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Queries;

namespace UniversityApp.AssignmentService.Controllers;

[ApiController]
[Route("/assignments/{assignmentId:guid}/submissions")]
[Authorize]
public class SubmissionController(ISubmissionService submissionService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAll(Guid assignmentId, [FromQuery] SubmissionQuery query)
	{
		var result = await submissionService.GetAllAsync(assignmentId, query);
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpGet("{submissionId:guid}")]
	public async Task<IActionResult> GetById(Guid assignmentId, Guid submissionId)
	{
		var result = await submissionService.GetByIdAsync(assignmentId, submissionId);
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpPost]
	[Authorize(Roles = "student")]
	public async Task<IActionResult> Create(Guid assignmentId, [FromBody] CreateSubmissionDto dto)
	{
		var result = await submissionService.CreateAsync(
			assignmentId,
			dto,
			Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value));
		return result.IsSuccess
			? Ok(result.Value)
			: BadRequest(result.Error);
	}
	
	[HttpPut("{submissionId:guid}")]
	[Authorize(Roles = "student")]
	public async Task<IActionResult> Update(Guid assignmentId, Guid submissionId, [FromBody] UpdateSubmissionDto dto)
	{
		var result = await submissionService.UpdateAsync(assignmentId, submissionId, dto);
		return result.IsSuccess
			? Ok(result.Value)
			: NotFound(result.Error);
	}
	
	[HttpDelete("{submissionId:guid}")]
	[Authorize(Roles = "student")]
	public async Task<IActionResult> Delete(Guid assignmentId, Guid submissionId)
	{
		var result = await submissionService.DeleteAsync(assignmentId, submissionId);
		return result.IsSuccess
			? NoContent()
			: NotFound(result.Error);
	}
}