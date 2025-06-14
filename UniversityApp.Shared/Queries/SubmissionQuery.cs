namespace UniversityApp.Shared.Queries;

public class SubmissionQuery : BaseQuery
{
	public Guid[]? StudentIds { get; init; }
}