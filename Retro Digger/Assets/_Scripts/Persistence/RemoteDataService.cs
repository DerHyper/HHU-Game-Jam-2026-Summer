using UnityEngine.Networking;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class RemoteDataService : IDataService
{
    private string _baseUrl = "";

    public RemoteDataService()
    {
        throw new NotImplementedException("This is a placeholder implementation. The RemoteDataService is not yet implemented and should be completed in the future to provide functionality for storing and retrieving data from a remote API or cloud storage.");
    }


    public async Task CreateAsync<T>(string id, T data) where T : new() => await SendRequest(id, "POST", data);
    public async Task UpdateAsync<T>(string id, T data) where T : new() => await SendRequest(id, "PUT", data);

    public async Task<T> ReadAsync<T>(string id) where T : new()
    {
        var request = UnityWebRequest.Get(_baseUrl + id);
        await request.SendWebRequest(); // Requires an Extension method or wrapper
        return JsonUtility.FromJson<T>(request.downloadHandler.text);
    }

    public async Task DeleteAsync(string id)
    {
        var request = UnityWebRequest.Delete(_baseUrl + id);
        await request.SendWebRequest();
    }

    private async Task SendRequest<T>(string id, string method, T data)
    {
        string json = JsonUtility.ToJson(data);
        using var request = new UnityWebRequest(_baseUrl + id, method);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        await request.SendWebRequest();
    }
}