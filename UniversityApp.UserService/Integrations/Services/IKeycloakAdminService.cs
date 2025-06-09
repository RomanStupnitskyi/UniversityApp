namespace UniversityApp.UserService.Integrations.Services;

public interface IKeycloakAdminService
{
	Task AssignRoleToUserAsync(string userId, string roleName);
}