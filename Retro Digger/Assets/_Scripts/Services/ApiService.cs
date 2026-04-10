using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ApiService
{
    static ApiService _instance;
    public static ApiService Instance => _instance ??= new ApiService();
    private ApiService() { }

    const string ApiEndpoint = "https://digger.jaessdev.de/api/v1/scores";

    public async Task<List<ScoreEntry>> GetScores(int limit = 5)
    {
        var url = $"{ApiEndpoint}?limit={Mathf.Max(1, limit)}";

        using var request = UnityWebRequest.Get(url);
        request.downloadHandler = new DownloadHandlerBuffer();

        await SendAsync(request);
        if (request.result != UnityWebRequest.Result.Success)
        {
            return new();
        }

        var json = request.downloadHandler.text;
        return JsonArrayHelper.FromJson<ScoreEntry>(json);
    }

    public async Task<ScoreEntry> CreateScore(string playerName, int points)
    {
        CreateScoreRequest payload = new()
        {
            playerName = (playerName ?? string.Empty).Trim(),
            points = Mathf.Max(0, points)
        };

        var json = JsonUtility.ToJson(payload);
        var bodyRaw = Encoding.UTF8.GetBytes(json);

        using UnityWebRequest request = new(ApiEndpoint, UnityWebRequest.kHttpVerbPOST);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        await SendAsync(request);
        if (request.result != UnityWebRequest.Result.Success)
        {
            return null;
        }

        var responseJson = request.downloadHandler.text;
        return JsonUtility.FromJson<ScoreEntry>(responseJson);
    }

    private static async Task SendAsync(UnityWebRequest request)
    {
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }
    }
}

[Serializable]
public class CreateScoreRequest
{
    public string playerName;
    public int points;
}

[Serializable]
public class ScoreEntry
{
    public string id;
    public string playerName;
    public int points;
}

public static class JsonArrayHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }

    public static List<T> FromJson<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<T>();
        }

        var wrapped = $"{{\"items\":{json}}}";
        var wrapper = JsonUtility.FromJson<Wrapper<T>>(wrapped);
        return wrapper?.items ?? new List<T>();
    }
}