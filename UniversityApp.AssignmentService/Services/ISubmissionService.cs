using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.AssignmentService.Services;

public interface ISubmissionService
{
	Task<ActionResult> GetAllAsync(string assignmentId);
	Task<ActionResult> GetByIdAsync(string assignmentId, string submissionId);
	Task<ActionResult> CreateAsync(string assignmentId, CreateSubmissionDto dto);
	Task<ActionResult> UpdateAsync(string assignmentId, string submissionId, UpdateSubmissionDto dto);
	Task<ActionResult> DeleteAsync(string assignmentId, string submissionId);
}