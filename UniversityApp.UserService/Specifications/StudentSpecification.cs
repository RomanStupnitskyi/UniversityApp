using Ardalis.Specification;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;
using UniversityApp.UserService.Extensions;

namespace UniversityApp.UserService.Specifications;

public class StudentSpecification : Specification<Student>
{
	public StudentSpecification(StudentQuery query)
	{
		// [#] Apply filters based on the query parameters
		if (query.Ids is { Length: > 0 })
		{
			Query.Where(student => query.Ids.Contains(student.Id));
		}
		
		// [#] Apply filters based on the contact numbers
		if (query.StudentNumbers is { Length: > 0 })
		{
			Query.Where(student => query.StudentNumbers.Contains(student.StudentNumber));
		}
		
		// [#] Apply ordering based on the query parameters
		switch (query.OrderBy)
		{
			case "studentNumber":
				Query.OrderByDirection(student => student.StudentNumber, query.Ascending);
				break;
			default:
				Query.OrderByDirection(student => student.CreatedAt, query.Ascending);
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