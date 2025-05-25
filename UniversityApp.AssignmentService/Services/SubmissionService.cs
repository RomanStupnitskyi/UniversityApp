using Microsoft.AspNetCore.Mvc;
using UniversityApp.AssignmentService.API;
using UniversityApp.AssignmentService.Repositories;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Services;

public class SubmissionService(
	ISubmissionRepository submissionRepository,
	IAssignmentRepository assignmentRepository,
	IUserAPI userAPI
	) : ISubmissionService
{
	public async Task<ActionResult> GetAllAsync(string assignmentId)
	{
		try
		{
			var guid = Guid.TryParse(assignmentId, out var parsedId);
			if (!guid)
				return new BadRequestObjectResult("AssignmentId must be a valid GUID.");
			if (parsedId == Guid.Empty)
				return new BadRequestObjectResult("AssignmentId must not be an empty GUID.");
			
			var assignment = await assignmentRepository.GetByIdAsync(assignmentId);
			if (assignment == null)
				return new NotFoundObjectResult($"Assignment with ID=\"{assignmentId}\" not found");
			
			var submissions = await submissionRepository.GetAllAsync(assignmentId);
			return new OkObjectResult(submissions);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to fetch submissions");
		}
	}

	public async Task<ActionResult> GetByIdAsync(string assignmentId, string submissionId)
	{
		try
		{
			var assignmentGuid = Guid.TryParse(assignmentId, out var assignmentParsedId);
			if (!assignmentGuid)
				return new BadRequestObjectResult("AssignmentId must be a valid GUID.");
			if (assignmentParsedId == Guid.Empty)
				return new BadRequestObjectResult("AssignmentId must not be an empty GUID.");
			
			var submissionGuid = Guid.TryParse(submissionId, out var submissionParsedId);
			if (!submissionGuid)
				return new BadRequestObjectResult("SubmissionId must be a valid GUID.");
			if (submissionParsedId == Guid.Empty)
				return new BadRequestObjectResult("SubmissionId must not be an empty GUID.");
			
			var submission = await submissionRepository.GetByIdAsync(assignmentId, submissionId);
			return new OkObjectResult(submission);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to fetch submission");
		}
	}

	public async Task<ActionResult> CreateAsync(string assignmentId, CreateSubmissionDto dto)
	{
		try
		{
			var guid = Guid.TryParse(assignmentId, out var parsedId);
			if (!guid)
				return new BadRequestObjectResult("AssignmentId must be a valid GUID.");
			if (parsedId == Guid.Empty)
				return new BadRequestObjectResult("AssignmentId must not be an empty GUID.");
			
			var submission = await submissionRepository.GetByIdAsync(assignmentId, dto.Id);
			if (submission != null)
				return new NotFoundObjectResult($"Submission with ID=\"{dto.Id}\" already exists");
			
			var response = await userAPI.GetStudentByIdAsync(dto.StudentId);
			if (!response.IsSuccessStatusCode)
				return new NotFoundObjectResult($"Student with ID=\"{dto.StudentId}\" not found");
			
			var submissionByStudent = await submissionRepository.GetByStudentIdAsync(assignmentId, dto.StudentId);
			if (submissionByStudent != null)
				return new BadRequestObjectResult($"Submission from StudentId=\"{dto.StudentId}\" in the AssignmentId=\"{assignmentId}\" already exists");

			var newSubmission = new Submission
			{
				Id = dto.Id,
				AssignmentId = assignmentId,
				StudentId = dto.StudentId,
				Content = dto.Content
			};
			var createdSubmission = await submissionRepository.AddAsync(newSubmission);
			return new OkObjectResult(createdSubmission);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to create submission");
		}
	}

	public async Task<ActionResult> UpdateAsync(string assignmentId, string submissionId, UpdateSubmissionDto dto)
	{
		try
		{
			var assignmentGuid = Guid.TryParse(assignmentId, out var assignmentParsedId);
			if (!assignmentGuid)
				return new BadRequestObjectResult("AssignmentId must be a valid GUID.");
			if (assignmentParsedId == Guid.Empty)
				return new BadRequestObjectResult("AssignmentId must not be an empty GUID.");
			
			var submissionGuid = Guid.TryParse(submissionId, out var submissionParsedId);
			if (!submissionGuid)
				return new BadRequestObjectResult("SubmissionId must be a valid GUID.");
			if (submissionParsedId == Guid.Empty)
				return new BadRequestObjectResult("SubmissionId must not be an empty GUID.");
			
			var assignment = await assignmentRepository.GetByIdAsync(assignmentId);
			if (assignment == null)
				return new NotFoundObjectResult($"Assignment with ID=\"{assignmentId}\" not found");
			
			var submission = await submissionRepository.GetByIdAsync(assignmentId, submissionId);
			if (submission == null)
				return new NotFoundObjectResult($"Submission with ID=\"{submissionId}\" not found");
			
			submission.Content = dto.Content;
			var updatedSubmission = await submissionRepository.UpdateAsync(submission);
			return new OkObjectResult(updatedSubmission);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to update submission");
		}
	}

	public async Task<ActionResult> DeleteAsync(string assignmentId, string submissionId)
	{
		try
		{
			var guid = Guid.TryParse(submissionId, out var parsedId);
			if (!guid)
				return new BadRequestObjectResult("SubmissionId must be a valid GUID.");
			if (parsedId == Guid.Empty)
				return new BadRequestObjectResult("SubmissionId must not be an empty GUID.");
			
			var submission = await submissionRepository.GetByIdAsync(assignmentId, submissionId);
			if (submission == null)
				return new NotFoundObjectResult($"Submission with ID=\"{submissionId}\" not found");
			
			var success = await submissionRepository.DeleteAsync(submission);
			return success
				? new OkObjectResult("Submission deleted successfully")
				: new ConflictObjectResult("Failed to delete submission");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to delete submission");
		}
	}
}