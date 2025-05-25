using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.AssignmentService.Services;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.AssignmentService.Controllers;

[ApiController]
[Route("/{assignmentId}/submissions")]
public class SubmissionController(ISubmissionService submissionService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAll(string assignmentId)
	{
		var result = await submissionService.GetAllAsync(assignmentId);
		return result;
	}
	
	[HttpGet("{submissionId}")]
	public async Task<IActionResult> GetById(string assignmentId, string submissionId)
	{
		var result = await submissionService.GetByIdAsync(assignmentId, submissionId);
		return result;
	}
	
	[HttpPost]
	public async Task<IActionResult> Create(string assignmentId, [FromBody] CreateSubmissionDto dto)
	{
		var result = await submissionService.CreateAsync(assignmentId, dto);
		return result;
	}
	
	[HttpPut("{submissionId}")]
	public async Task<IActionResult> Update(string assignmentId, string submissionId, [FromBody] UpdateSubmissionDto dto)
	{
		var result = await submissionService.UpdateAsync(assignmentId, submissionId, dto);
		return result;
	}
	
	[HttpDelete("{submissionId}")]
	[SuppressMessage("ReSharper", "UnusedParameter.Global")]
	public async Task<IActionResult> Delete(string assignmentId, string submissionId)
	{
		var result = await submissionService.DeleteAsync(assignmentId, submissionId);
		return result;
	}
}