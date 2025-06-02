namespace UniversityApp.Shared.Queries;

public class AssignmentQuery : BaseQuery
{
	public Guid? Id { get; set; }
	public Guid? CourseId { get; set; }
	public bool? Active { get; set; }
}