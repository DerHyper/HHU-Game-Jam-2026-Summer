using System.Threading.Tasks;

/// <summary>
/// Defines a simple interface for basic CRUD (Create, Read, Update, Delete) operations on a data storage system.
/// </summary>
public interface IDataService
{
    /// <summary>
    /// Creates or updates a data entry with the given data. The entry is identified by the provided id. If an entry with the same id already exists, it will be overwritten.
    /// </summary>
    /// <typeparam name="T">The type of the data to be stored.</typeparam>
    /// <param name="id">The unique identifier for the data entry.</param>
    /// <param name="data">The data to be stored.</param>
    Task CreateAsync<T>(string id, T data) where T : new();


    /// <summary>
    /// Reads a data entry identified by the provided id. If the entry does not exist, it returns the default value of type T.
    /// </summary>
    /// <typeparam name="T">The type of the data to be read.</typeparam>
    /// <param name="id">The unique identifier for the data entry.</param>
    /// <returns>The data entry if it exists; otherwise, the default value of type T.</returns>
    Task<T> ReadAsync<T>(string id) where T : new();

    /// <summary>
    /// Updates a data entry identified by the provided id with the given data. If the entry does not exist, it will be created.
    /// </summary>
    /// <typeparam name="T">The type of the data to be updated.</typeparam>
    /// <param name="id">The unique identifier for the data entry.</param>
    /// <param name="data">The data to be updated.</param>
    Task UpdateAsync<T>(string id, T data) where T : new();


    /// <summary>
    /// Deletes a data entry identified by the provided id. If the entry does not exist, it does nothing.
    /// </summary>
    /// <param name="id">The unique identifier for the data entry.</param>
    Task DeleteAsync(string id);
}