using Ardalis.Specification;
using UniversityApp.AssignmentService.Extensions;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;

namespace UniversityApp.AssignmentService.Specifications;

public class AssignmentSpecification : Specification<Assignment>
{
	public AssignmentSpecification(AssignmentQuery query)
	{
		// [#] Apply filters based on the query parameters
		if (query.Ids is { Length: > 0 })
		{
			Query.Where(assignment => query.Ids.Contains(assignment.Id));
		}

		if (query.CourseIds is { Length: > 0 })
		{
			Query.Where(assignment => query.CourseIds.Contains(assignment.CourseId));
		}

		if (query.Active.HasValue)
		{
			Query.Where(assignment => assignment.EndDate == null || assignment.EndDate > DateTime.UtcNow);
		}
		
		// [#] Apply ordering based on the query parameters
		switch (query.OrderBy)
		{
			case "title":
				Query.OrderByDirection(assignment => assignment.Title, query.Ascending);
				break;
			case "startDate":
				Query.OrderByDirection(assignment => assignment.StartDate, query.Ascending);
				break;
			case "endDate":
				Query.OrderByDirection(assignment => assignment.EndDate, query.Ascending);
				break;
			default:
				Query.OrderByDirection(assignment => assignment.CreatedAt, query.Ascending);
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