using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Repositories;

public interface ISubmissionRepository
{
	Task<IEnumerable<Submission>> GetAllAsync(string assignmentId);
	Task<Submission?> GetByIdAsync(string assignmentId, string submissionId);
	Task<Submission?> GetByStudentIdAsync(string assignmentId, string studentId);
	Task<bool> AddAsync(Submission submission);
	Task<bool> UpdateAsync(Submission submission);
	Task<bool> DeleteAsync(Submission submission);
	Task<bool> DeleteByIdAsync(string submissionId);
}