using Ardalis.Specification;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;
using UniversityApp.UserService.Extensions;

namespace UniversityApp.UserService.Specifications;

public class LecturerSpecification : Specification<Lecturer>
{
	public LecturerSpecification(LecturerQuery query)
	{
		// [#] Apply filters based on the query parameters
		if (query.Ids is { Length: > 0 })
		{
			Query.Where(lecturer => query.Ids.Contains(lecturer.Id));
		}
		
		// [#] Apply filters based on the contact numbers
		if (query.ContactNumbers is { Length: > 0 })
		{
			Query.Where(lecturer => query.ContactNumbers.Contains(lecturer.ContactNumber));
		}
		
		// [#] Apply filters based on the contact emails
		if (query.ContactEmails is { Length: > 0 })
		{
			Query.Where(lecturer => query.ContactEmails.Contains(lecturer.ContactEmail));
		}
		
		// [#] Apply ordering based on the query parameters
		switch (query.OrderBy)
		{
			case "contactNumber":
				Query.OrderByDirection(lecturer => lecturer.ContactNumber, query.Ascending);
				break;
			case "contactEmail":
				Query.OrderByDirection(lecturer => lecturer.ContactEmail, query.Ascending);
				break;
			default:
				Query.OrderByDirection(lecturer => lecturer.CreatedAt, query.Ascending);
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