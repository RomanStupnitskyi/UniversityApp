namespace UniversityApp.Shared.Queries;

public class AssignmentQuery : BaseQuery
{
	public Guid[]? CourseIds { get; set; }
	public bool? Active { get; set; }
}