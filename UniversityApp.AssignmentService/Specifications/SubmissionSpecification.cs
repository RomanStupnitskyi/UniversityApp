using Ardalis.Specification;
using UniversityApp.AssignmentService.Extensions;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;

namespace UniversityApp.AssignmentService.Specifications;

public class SubmissionSpecification : Specification<Submission>
{
	public SubmissionSpecification(SubmissionQuery query)
	{
		// [#] Apply filters based on the query parameters
		if (query.Ids is { Length: > 0 })
		{
			Query.Where(submission => query.Ids.Contains(submission.Id));
		}
		
		// [#] Apply filters based in StudentIds
		if (query.StudentIds is { Length: > 0 })
		{
			Query.Where(submission => query.StudentIds.Contains(submission.StudentId));
		}
		
		// [#] Apply ordering based on the query parameters
		switch (query.OrderBy)
		{
			case "content":
				Query.OrderByDirection(submission => submission.Content, query.Ascending);
				break;
			case "lastUpdatedAt":
				Query.OrderByDirection(submission => submission.LastUpdatedAt, query.Ascending);
				break;
			default:
				Query.OrderByDirection(submission => submission.SubmittedAt, query.Ascending);
				break;
		}
		
		// [#] Apply pagination based on the query parameters
		if (query is { Page: > 0, PageSize: > 0 })
		{
			Query.Skip((query.Page - 1) * query.PageSize)
				.Take(query.PageSize);
		}
	}
}