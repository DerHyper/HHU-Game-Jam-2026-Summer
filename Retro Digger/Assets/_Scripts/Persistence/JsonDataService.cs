using System.IO;
using UnityEngine;
using System.Threading.Tasks;

/// <summary>
/// A simple implementation of IDataService that uses JSON files for storage. 
/// Each data entry is stored in a separate JSON file named after the provided id, 
/// located in the application's persistent data path. 
/// This service is suitable for small amounts of data and is easy to use without requiring additional dependencies. 
/// However, it may not be the best choice for large datasets or complex queries due to performance limitations 
/// and lack of advanced features compared to databases like SQLite or remote APIs.
/// </summary>
/// <remarks>Due to the nature of JsonUtility, the type T must be annotated like a normal Serializable class in Unity.
/// Otherwise, fields may be missing.</remarks>
public class JsonDataService : IDataService
{
    private string GetPath(string id) => Path.Combine(Application.persistentDataPath, $"{id}.json");

    /// <summary>
    /// Creates or updates a JSON file with the given data. The file is named based on the provided id and stored in the persistent data path of the application.
    /// </summary>
    /// <typeparam name="T">The type to be serialized and stored in the JSON file.</typeparam>
    /// <param name="id">The id used to name the JSON file.</param>
    /// <param name="data">The data to be serialized and stored in the JSON file.</param>
    /// <remarks>Due to the nature of JsonUtility, the type T must be annotated like a normal Serializable class in Unity.
    /// Both Unity and the serializer adhere to the same rules for serialization.</remarks>
    public async Task CreateAsync<T>(string id, T data) where T : new() => await UpdateAsync(id, data);

    /// <summary>
    /// Reads a JSON file based on the provided id and deserializes it into an object of type T. If the file does not exist, it returns the default values in type T.
    /// Additional fields are ignored.
    /// </summary>
    /// <typeparam name="T">The type to be deserialized from the JSON file.</typeparam>
    /// <param name="id">The id used to locate the JSON file.</param>
    /// <returns>The deserialized object of type T, or the default values if the file does not exist.</returns>
    public async Task<T> ReadAsync<T>(string id) where T : new()
    {
        string path = GetPath(id);
        if (!File.Exists(path)) return default;
        string json = await File.ReadAllTextAsync(path);
        return JsonUtility.FromJson<T>(json);
    }

    /// <summary>
    /// Updates a JSON file with the given data. If the file does not exist, it creates a new one. The file is named based on the provided id and stored in the persistent data path of the application.
    /// </summary>
    /// <typeparam name="T">The type to be serialized and stored in the JSON file.</typeparam>
    /// <param name="id">The id used to name the JSON file.</param>
    /// <param name="data">The data to be serialized and stored in the JSON file.</param>
    public async Task UpdateAsync<T>(string id, T data) where T : new()
    {
        string json = JsonUtility.ToJson(data);
        await File.WriteAllTextAsync(GetPath(id), json);
    }

    /// <summary>
    /// Deletes the JSON file associated with the provided id from the persistent data path of the application. If the file does not exist, it does nothing.
    /// </summary>
    /// <param name="id">The id used to locate the JSON file.</param>
    public async Task DeleteAsync(string id)
    {
        if (File.Exists(GetPath(id))) File.Delete(GetPath(id));
        await Task.CompletedTask;
    }
}