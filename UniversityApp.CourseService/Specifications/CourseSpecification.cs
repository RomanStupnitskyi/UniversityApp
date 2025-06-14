using Ardalis.Specification;
using UniversityApp.CourseService.Extensions;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;

namespace UniversityApp.CourseService.Specifications;

public class CourseSpecification : Specification<Course>
{
	private CourseQuery _query;
	public CourseSpecification(CourseQuery query)
	{
		_query = query;
		
		// [#] Apply filters based on the query parameters
		if (query.Ids is { Length: > 0 })
		{
			Query.Where(course => query.Ids.Contains(course.Id));
		}
		
		// [#] Apply filters based on the ECTS
		if (query.ECTS != null)
		{
			Query.Where(course => course.ECTS == query.ECTS);
		}
		
		// [#] Apply ordering based on the query parameters
		switch (query.OrderBy)
		{
			case "title":
				Query.OrderByDirection(course => course.Title, query.Ascending);
				break;
			case "description":
				Query.OrderByDirection(course => course.Description, query.Ascending);
				break;
			case "ects":
				Query.OrderByDirection(course => course.ECTS, query.Ascending);
				break;
			default:
				Query.OrderByDirection(course => course.CreatedAt, query.Ascending);
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