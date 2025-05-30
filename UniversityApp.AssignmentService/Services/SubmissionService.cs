using CSharpFunctionalExtensions;
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
	public async Task<Result<IEnumerable<Submission>>> GetAllAsync(Guid assignmentId)
	{
		var assignment = await assignmentRepository.GetByIdAsync(assignmentId);
		if (assignment == null)
			return Result.Failure<IEnumerable<Submission>>($"Assignment with ID=\"{assignmentId}\" not found");
		
		var submissions = await submissionRepository.GetAllAsync(assignmentId);
		return Result.Success(submissions);
	}

	public async Task<Result<Submission?>> GetByIdAsync(Guid assignmentId, Guid submissionId)
	{
		var submission = await submissionRepository.GetByIdAsync(assignmentId, submissionId);
		return Result.Success(submission);
	}

	public async Task<Result<Submission>> CreateAsync(Guid assignmentId, CreateSubmissionDto dto)
	{
		var submission = await submissionRepository.GetByIdAsync(assignmentId, dto.Id);
		if (submission != null)
			return Result.Failure<Submission>($"Submission with ID=\"{dto.Id}\" already exists");
		
		var response = await userAPI.GetStudentByIdAsync(dto.StudentId);
		if (!response.IsSuccessStatusCode)
			return Result.Failure<Submission>($"Student with ID=\"{dto.StudentId}\" not found");
		
		var submissionByStudent = await submissionRepository.GetByStudentIdAsync(assignmentId, dto.StudentId);
		if (submissionByStudent != null)
			return Result.Failure<Submission>($"Submission from StudentId=\"{dto.StudentId}\" in the AssignmentId=\"{assignmentId}\" already exists");

		var newSubmission = new Submission
		{
			Id = dto.Id,
			AssignmentId = assignmentId,
			StudentId = dto.StudentId,
			Content = dto.Content
		};
		await submissionRepository.AddAsync(newSubmission);
		return Result.Success(newSubmission);
	}

	public async Task<Result<Submission>> UpdateAsync(Guid assignmentId, Guid submissionId, UpdateSubmissionDto dto)
	{
		var assignment = await assignmentRepository.GetByIdAsync(assignmentId);
		if (assignment == null)
			return Result.Failure<Submission>($"Assignment with ID=\"{assignmentId}\" not found");
		
		var submission = await submissionRepository.GetByIdAsync(assignmentId, submissionId);
		if (submission == null)
			return Result.Failure<Submission>($"Submission with ID=\"{submissionId}\" not found");
		
		submission.Content = dto.Content;
		await submissionRepository.UpdateAsync(submission);
		return Result.Success(submission);
	}

	public async Task<Result> DeleteAsync(Guid assignmentId, Guid submissionId)
	{
		var submission = await submissionRepository.GetByIdAsync(assignmentId, submissionId);
		if (submission == null)
			return Result.Failure($"Submission with ID=\"{submissionId}\" not found");
		
		await submissionRepository.DeleteAsync(submission);
		return Result.Success();
	}

	public async Task<Result> DeleteByAssignmentIdsAsync(IEnumerable<Guid> assignmentIds)
	{
		var submissionIdsList = assignmentIds.ToList();
		if (submissionIdsList.Count == 0)
			return Result.Failure("No assignment IDs provided for deletion");

		var result = await submissionRepository.DeleteByAssignmentIdsAsync(submissionIdsList);
		return result
			? Result.Success()
			: Result.Failure("Failed to delete submissions with the provided assignment IDs");
	}

	public async Task<Result> DeleteByAssignmentIdAsync(Guid assignmentId)
	{
		var assignment = await assignmentRepository.GetByIdAsync(assignmentId);
		if (assignment == null)
			return Result.Failure($"Assignment with ID=\"{assignmentId}\" not found");
		
		var submissions = await submissionRepository.GetAllAsync(assignmentId);
		if (!submissions.Any())
			return Result.Success(); // No submissions to delete
		
		await submissionRepository.DeleteByAssignmentIdAsync(assignmentId);
		return Result.Success();
	}
}