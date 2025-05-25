using Microsoft.EntityFrameworkCore;
using UniversityApp.AssignmentService.Data;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Repositories;

public class SubmissionRepository(AssignmentDbContext assignmentDbContext) : ISubmissionRepository
{
	public async Task<IEnumerable<Submission>> GetAllAsync(string assignmentId)
	{
		var submissions = await assignmentDbContext.Submissions
			.Where(s => s.AssignmentId == assignmentId)
			.ToListAsync();
		return submissions;
	}

	public async Task<Submission?> GetByIdAsync(string assignmentId, string submissionId)
	{
		var submission = await assignmentDbContext.Submissions
			.FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.Id == submissionId);
		return submission;
	}

	public async Task<Submission?> GetByStudentIdAsync(string assignmentId, string studentId)
	{
		var submission = await assignmentDbContext.Submissions
			.FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId);
		return submission;
	}

	public async Task<bool> AddAsync(Submission submission)
	{
		try
		{
			await assignmentDbContext.Submissions.AddAsync(submission);
			await assignmentDbContext.SaveChangesAsync();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> UpdateAsync(Submission submission)
	{
		try
		{
			submission.LastUpdatedAt = DateTime.UtcNow;
			assignmentDbContext.Submissions.Update(submission);
			await assignmentDbContext.SaveChangesAsync();
			
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> DeleteAsync(Submission submission)
	{
		try
		{
			assignmentDbContext.Submissions.Remove(submission);
			await assignmentDbContext.SaveChangesAsync();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> DeleteByIdAsync(string submissionId)
	{
		var submission = await assignmentDbContext.Submissions
			.FirstOrDefaultAsync(s => s.Id == submissionId);
		if (submission == null)
			throw new KeyNotFoundException($"Submission with ID=\"{submissionId}\" not found.");

		try
		{
			assignmentDbContext.Submissions.Remove(submission);
			await assignmentDbContext.SaveChangesAsync();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}