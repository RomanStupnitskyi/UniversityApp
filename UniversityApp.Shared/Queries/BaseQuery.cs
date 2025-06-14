namespace UniversityApp.Shared.Queries;

public class BaseQuery
{
	public Guid[]? Ids { get; set; }
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public string OrderBy { get; set; } = "createdAt";
	public bool Ascending { get; set; } = false;
}