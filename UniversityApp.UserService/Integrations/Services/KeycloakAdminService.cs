using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text.Json;

namespace UniversityApp.UserService.Integrations.Services;

[SuppressMessage("ReSharper", "NotResolvedInText")]
public class KeycloakAdminService(HttpClient httpClient, IConfiguration config) : IKeycloakAdminService
{
    private async Task<string> GetAdminAccessTokenAsync()
    {
        var tokenUrl = $"{config["Keycloak:Authority"]}/realms/master/protocol/openid-connect/token";

        var form = new Dictionary<string, string>
        {
            { "client_id", config["Keycloak:AdminClientId"] ?? throw new ArgumentNullException("Keycloak:AdminClientId") },
            { "client_secret", config["Keycloak:AdminClientSecret"] ?? throw new ArgumentNullException("Keycloak:AdminClientSecret") },
            { "grant_type", "client_credentials" }
        };

        var response = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(form));
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("access_token").GetString()
            ?? throw new InvalidOperationException("Access token not found in Keycloak response.");
    }

    public async Task AssignRoleToUserAsync(string userId, string roleName)
    {
        var accessToken = await GetAdminAccessTokenAsync();

        var clientId = await GetClientUuid(accessToken);
        var role = await GetClientRole(clientId, roleName, accessToken);

        var assignRoleUrl = $"{config["Keycloak:Authority"]}/admin/realms/{config["Keycloak:Realm"]}/users/{userId}/role-mappings/clients/{clientId}";

        var request = new HttpRequestMessage(HttpMethod.Post, assignRoleUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = JsonContent.Create(new[] { role });

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    private async Task<string> GetClientUuid(string token)
    {
        var clientsUrl = $"{config["Keycloak:Authority"]}/admin/realms/{config["Keycloak:Realm"]}/clients";
        var request = new HttpRequestMessage(HttpMethod.Get, clientsUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.SendAsync(request);
        var clients = await response.Content.ReadFromJsonAsync<List<JsonElement>>();
        
        if (clients == null || clients.Count == 0)
            throw new InvalidOperationException("No clients found in Keycloak realm.");

        return clients.First(c => c.GetProperty("clientId").GetString() == config["Keycloak:FrontendClientId"])
                      .GetProperty("id").GetString() ?? "";
    }

    private async Task<object> GetClientRole(string clientId, string roleName, string token)
    {
        var rolesUrl = $"{config["Keycloak:Authority"]}/admin/realms/{config["Keycloak:Realm"]}/clients/{clientId}/roles/{roleName}";
        var request = new HttpRequestMessage(HttpMethod.Get, rolesUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.SendAsync(request);
        var role = await response.Content.ReadFromJsonAsync<JsonElement>();

        return new
        {
            id = role.GetProperty("id").GetString(),
            name = role.GetProperty("name").GetString()
        };
    }
}
