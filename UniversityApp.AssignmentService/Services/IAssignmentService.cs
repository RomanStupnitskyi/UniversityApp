using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.AssignmentService.Services;

public interface IAssignmentService
{
	Task<ActionResult> GetAllAsync();
	Task<ActionResult> GetByIdAsync(string id);
	Task<ActionResult> CreateAsync(CreateAssignmentDto dto);
	Task<ActionResult> UpdateAsync(string id, UpdateAssignmentDto dto);
	Task<ActionResult> DeleteAsync(string id);
}