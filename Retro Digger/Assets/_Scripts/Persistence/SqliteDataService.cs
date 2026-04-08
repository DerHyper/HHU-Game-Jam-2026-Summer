using System.Threading.Tasks;
using SQLite;

public class SqliteDataService : IDataService
{
    private readonly SQLiteAsyncConnection _db;

    public SqliteDataService(string dbPath)
    {
        _db = new(dbPath);
    }

    /// <summary>
    /// Creates a new record in the SQLite database for the specified type and data.
    /// If no table exists for the type T, it will be created automatically. If a record with the same id already exists, it will be overwritten.
    /// </summary>
    /// <typeparam name="T">The type of the data to be stored.</typeparam>
    /// <param name="id">The id of the object to be stored.</param>
    /// <param name="data">The actual data to be stored.</param>
    public async Task CreateAsync<T>(string id, T data) where T : new()
    {
        await _db.CreateTableAsync<T>();
        await _db.InsertAsync(data);
    }

    /// <summary>
    /// Reads a record from the SQLite database based on the provided id and deserializes it into an object of type T. If the record does not exist, it returns the default values in type T.
    /// </summary>
    /// <typeparam name="T">The type of the object to be read.</typeparam>
    /// <param name="id">The id of the object to be read.</param>
    /// <returns>The object of type T with the specified id, or throws an <see cref="SQLiteException"/> if not found.</returns>
    public async Task<T> ReadAsync<T>(string id) where T : new()
        => await _db.FindAsync<T>(id);

    /// <summary>
    /// Updates an existing record in the SQLite database for the specified type and data. If the record does not exist, it will be created. The record is identified by the provided id.
    /// </summary>
    /// <typeparam name="T">The type of the object to be updated.</typeparam>
    /// <param name="id">The id of the object to be updated.</param>
    /// <param name="data">The data to be updated.</param>
    public async Task UpdateAsync<T>(string id, T data) where T : new() => await _db.UpdateAsync(data);

    /// <summary>
    /// Deletes a record from the SQLite database based on the provided id.
    /// </summary>
    /// <param name="id">The id of the object to be deleted.</param>
    public async Task DeleteAsync(string id) => await _db.DeleteAsync<object>(id);
}