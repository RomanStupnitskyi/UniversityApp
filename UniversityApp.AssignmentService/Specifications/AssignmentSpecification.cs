using Ardalis.Specification;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;

namespace UniversityApp.AssignmentService.Specifications;

public class AssignmentSpecification : Specification<Assignment>
{
	public AssignmentSpecification(AssignmentQuery query)
	{
		// [#] Apply filters based on the query parameters
		if (query.Id.HasValue)
		{
			Query.Where(a => a.Id == query.Id.Value);
		}

		if (query.CourseId.HasValue)
		{
			Query.Where(a => a.CourseId == query.CourseId.Value);
		}

		if (query.Active.HasValue)
		{
			Query.Where(a => a.EndDate == null || a.EndDate > DateTime.UtcNow);
		}
		
		// [#] Apply ordering based on the query parameters
		switch (query.OrderBy)
		{
			case "title":
				Query.OrderBy(a => a.Title);
				break;
			case "startDate":
				Query.OrderBy(a => a.StartDate);
				break;
			case "endDate":
				Query.OrderBy(a => a.EndDate);
				break;
			default:
				Query.OrderBy(a => a.CreatedAt);
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