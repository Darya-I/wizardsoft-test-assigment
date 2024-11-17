using System.Net.Http.Json;
using System.Text.Json;

public class TreeApiClient
{
    private readonly HttpClient _httpClient;

    public TreeApiClient(string baseUrl)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    public async Task<List<TreeNode>> GetTreeAsync()
    {
        var response = await _httpClient.GetAsync("api/Tree");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<TreeNode>>() ?? new List<TreeNode>();
    }

    public async Task<TreeNode?> GetNodeAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/Tree/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<TreeNode>();
    }

    public async Task<TreeNode?> CreateNodeAsync(TreeNode node)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Tree", node);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<TreeNode>();
    }

    public async Task<bool> UpdateNodeAsync(TreeNode node)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Tree/{node.Id}", node);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteNodeAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Tree/{id}");
        return response.IsSuccessStatusCode;
    }
}
