using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;

namespace UniversityApp.AssignmentService.Services;

public interface ISubmissionService
{
	Task<Result<IEnumerable<Submission>>> GetAllAsync(Guid assignmentId, SubmissionQuery query);
	Task<Result<Submission?>> GetByIdAsync(Guid assignmentId, Guid submissionId);
	Task<Result<Submission>> CreateAsync(Guid assignmentId, CreateSubmissionDto dto, Guid studentId);
	Task<Result<Submission>> UpdateAsync(Guid assignmentId, Guid submissionId, UpdateSubmissionDto dto);
	Task<Result> DeleteAsync(Guid assignmentId, Guid submissionId);
	Task<Result> DeleteByAssignmentIdAsync(Guid assignmentId);
	Task<Result> DeleteByAssignmentIdsAsync(IEnumerable<Guid> submissionIds);
}