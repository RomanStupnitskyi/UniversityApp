﻿namespace UniversityApp.Shared.DTOs;

public class UpdateAssignmentDto
{
	public string? Title { get; set; }
	public string? Description { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
}