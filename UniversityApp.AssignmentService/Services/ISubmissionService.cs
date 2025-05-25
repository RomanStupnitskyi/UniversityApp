using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Services;

public interface ISubmissionService
{
	Task<Result<IEnumerable<Submission>>> GetAllAsync(Guid assignmentId);
	Task<Result<Submission?>> GetByIdAsync(Guid assignmentId, Guid submissionId);
	Task<Result<Submission>> CreateAsync(Guid assignmentId, CreateSubmissionDto dto);
	Task<Result<Submission>> UpdateAsync(Guid assignmentId, Guid submissionId, UpdateSubmissionDto dto);
	Task<Result> DeleteAsync(Guid assignmentId, Guid submissionId);
}