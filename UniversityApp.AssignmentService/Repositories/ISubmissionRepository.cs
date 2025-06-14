using UniversityApp.AssignmentService.Specifications;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Repositories;

public interface ISubmissionRepository
{
	Task<IEnumerable<Submission>> GetAllAsync(Guid assignmentId, SubmissionSpecification specification);
	Task<Submission?> GetByIdAsync(Guid assignmentId, Guid submissionId);
	Task<Submission?> GetByStudentIdAsync(Guid assignmentId, Guid studentId);
	Task AddAsync(Submission submission);
	Task UpdateAsync(Submission submission);
	Task DeleteAsync(Submission submission);
	Task DeleteByAssignmentIdAsync(Guid assignmentId);
	Task<bool> DeleteByIdAsync(Guid submissionId);
	Task<bool> DeleteByAssignmentIdsAsync(IEnumerable<Guid> submissionId);
}