﻿namespace UniversityApp.Shared.Queries;

public class BaseQuery
{
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public string OrderBy { get; set; } = "createdAt";
}