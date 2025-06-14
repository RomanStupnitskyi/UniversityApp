using System.Linq.Expressions;
using Ardalis.Specification;

namespace UniversityApp.AssignmentService.Extensions;

public static class SpecificationBuilder
{
	public static IOrderedSpecificationBuilder<T> OrderByDirection<T>(
		this ISpecificationBuilder<T> builder,
		Expression<Func<T,object?>> expression,
		bool ascending = false)
	{
		return ascending switch
		{
			true => builder.OrderBy(expression),
			false => builder.OrderByDescending(expression)
		};
	}
}