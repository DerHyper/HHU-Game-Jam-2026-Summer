using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    #region Singleton Implementation
    public static DatabaseManager Instance { get; private set; }
    private DatabaseManager() { }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("DatabaseManager: Multiple instances detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    #endregion


    /// <summary>
    /// Provides access to a global data service that can be used for storing and retrieving data that 
    /// should persist across different runs of the game . 
    /// </summary>
    /// <remarks> In the future, this should also be persistent across different devices, for example by using a remote API or cloud storage. 
    /// </remarks>
    public IDataService GlobalDataService { get; private set; }
    /// <summary>
    /// Provides access to a local data service that can be used for storing and retrieving data that should only persist locally on the device.
    /// </summary>
    public IDataService LocalDataService { get; private set; }




    /// <summary>
    /// Initializes the database manager by setting up the local and global data services. 
    /// Both are initialized as SQLite databases with a fallback to JSON-based data stores.
    /// </summary>
    void Start()
    {
        LocalDataService = InitOrFallback($"{Application.persistentDataPath}/RetroDb.db");
        GlobalDataService = InitOrFallback($"{Application.persistentDataPath}/RetroDb.Global.db");
    }


    private IDataService InitOrFallback(string dbPath)
    {
        try
        {
            return new SqliteDataService(dbPath);
        }
        catch
        {
            Debug.LogError("Failed to initialize SqliteDataService. Falling back to JsonDataService. This may lead to performance issues and is not recommended for production use.");
            return new JsonDataService();
        }
    }
}
