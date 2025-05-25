using Microsoft.EntityFrameworkCore;
using UniversityApp.AssignmentService.Data;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Repositories;

public class SubmissionRepository(AssignmentDbContext assignmentDbContext) : ISubmissionRepository
{
	public async Task<IEnumerable<Submission>> GetAllAsync(Guid assignmentId)
	{
		var submissions = await assignmentDbContext.Submissions
			.Where(s => s.AssignmentId == assignmentId)
			.ToListAsync();
		return submissions;
	}

	public async Task<Submission?> GetByIdAsync(Guid assignmentId, Guid submissionId)
	{
		var submission = await assignmentDbContext.Submissions
			.FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.Id == submissionId);
		return submission;
	}

	public async Task<Submission?> GetByStudentIdAsync(Guid assignmentId, Guid studentId)
	{
		var submission = await assignmentDbContext.Submissions
			.FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId);
		return submission;
	}

	public async Task AddAsync(Submission submission)
	{
		await assignmentDbContext.Submissions.AddAsync(submission);
		await assignmentDbContext.SaveChangesAsync();
	}

	public async Task UpdateAsync(Submission submission)
	{
		submission.LastUpdatedAt = DateTime.UtcNow;
		assignmentDbContext.Submissions.Update(submission);
		await assignmentDbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(Submission submission)
	{
		assignmentDbContext.Submissions.Remove(submission);
		await assignmentDbContext.SaveChangesAsync();
	}

	public async Task DeleteByAssignmentIdAsync(Guid assignmentId)
	{
		var submissions = await assignmentDbContext.Submissions
			.Where(s => s.AssignmentId == assignmentId)
			.ToListAsync();
		
		assignmentDbContext.Submissions.RemoveRange(submissions);
		await assignmentDbContext.SaveChangesAsync();
	}

	public async Task<bool> DeleteByIdAsync(Guid submissionId)
	{
		var submission = await assignmentDbContext.Submissions
			.FirstOrDefaultAsync(s => s.Id == submissionId);
		if (submission == null)
			throw new KeyNotFoundException($"Submission with ID=\"{submissionId}\" not found.");

		assignmentDbContext.Submissions.Remove(submission);
		await assignmentDbContext.SaveChangesAsync();
		return true;
	}
}