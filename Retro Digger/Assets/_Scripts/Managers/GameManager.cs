using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Flow:
/// Start at Main Menu -[Start Button]-> Opens Tutorial -[Let's Go Button]->
/// Map View -[Hit rock] -> Digging View -[Finish Digging] -> Map View 
/// <Times over> -> Go to Shop View
/// Buy better tools with money
/// Next day, start at Map View again.
/// <3 days passed> -> Main Menu View
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    public int CurrentDay => TimeManager.Instance.CurrentDay;

    public MusicTrack MainMenuMusic;
    public MusicTrack MapDigMusic;
    public MusicTrack ShopMusic;
    public MusicTrack GameFinishedMusic;
    private RockCollider _currentRock = null;
    public RockCollider CurrentRock
    {
        get => _currentRock;
        set => _currentRock = value;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    void Start()
    {
        AudioManager.Instance.PlayMusic(MainMenuMusic);
        WhenSceneLoads(GameScene.DiggingView, (scene) =>
        {
            int level = GetLevelForDigging();
            CollectableManager.Instance.SpawnCollectable(level);
            DiggingManager.Instance.SpawnDirtSpots(level);
            InventoryManager.Instance.UpdateInventoryUI();
        });

        WhenSceneLoads(GameScene.MapView, (scene) =>
        {
            LevelManager.Instance.LoadLevel(TimeManager.Instance.CurrentDay - 1);
        });
    }

    private int GetLevelForDigging()
    {
        int maxLevel = ToolService.Instance.GetCurrentToolLevel();
        int level = UnityEngine.Random.Range(1, maxLevel+1);
        return level;
    }

    public void GoToMainMenu()
    {
        GameScene.MainMenu.LoadSingle();
    }

    /// <summary>
    /// Starts a new run of the game, resetting all progress. 
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Starting Game!");

        TimeManager.Instance.CurrentDay = 1;
        TimeManager.Instance.StartDay();
        GoToMap();
    }

    public void NextDay()
    {
        if (TimeManager.Instance.IsLastDay())
        {
            GoToGameFinished();
            return;
        }

        TimeManager.Instance.CurrentDay++;
        TimeManager.Instance.StartDay();

        GoToMap();
    }

    public void GoToMap()
    {
        if (!GameScene.DiggingView.IsLatestScene())
        {
            Debug.Log("Replacing with map view");
            AudioManager.Instance.PlayMusic(MapDigMusic);
            GameScene.MapView.LoadSingle();
            return;
        }

        Debug.Log("Popping the digging view");
        SceneManager.UnloadSceneAsync(GameScene.DiggingView.ScenePath).completed += (op) =>
        {
            if (GameScene.MapView.IsLatestScene())
            {
                Debug.Log("Map view already active, no need to load it again.");
                return;
            }

            Debug.Log("Map view was not active even though we came from digging!");
            GameScene.MapView.LoadAdditive();
        };
    }

    public void GoToDiggingView()
    {
        if (GameScene.MapView.IsLatestScene())
        {
            Debug.Log("Pushing the digging view");
            GameScene.DiggingView.LoadAdditive();
        }
        else
        {
            Debug.Log("Replacing with digging view");
            GameScene.DiggingView.LoadSingle();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void EndDiggingWon()
    {
        Collectable currentCollectable = CollectableManager.Instance?.CurrentCollectable;
        try
        {
            MoneyManager.Instance.AddMoneyAndScore(currentCollectable.GetCurrentValue());
        }
        catch (System.Exception)
        {
            Debug.LogWarning("MoneyManager not found, skipping money reward.");
        }
        GoToMap();
        DestroyCurrentRock();
    }

    /// <summary>
    /// 
    /// </summary>
    public void EndDiggingLost()
    {
        Invoke(nameof(GoToMap), 2f);
        DestroyCurrentRock();
    }

    internal void EndDiggingAborted()
    {
        EndDiggingLost();
    }

    public void EndGame()
    {
        Debug.Log("Game Ended!");
    }

    public void DestroyCurrentRock()
    {
        if (CurrentRock == null) return;
        CurrentRock.DestroyRock();
        CurrentRock = null;
    }

    public void WhenSceneUnloads(GameScene scene, Action<Scene> onUnload)
    {
        SceneManager.sceneUnloaded += (unloadedScene) =>
        {
            if (unloadedScene.name == scene.SceneName)
                onUnload(unloadedScene);
        };
    }

    public void WhenSceneLoads(GameScene scene, Action<Scene> onLoad)
    {
        SceneManager.sceneLoaded += (loadedScene, mode) =>
        {
            if (loadedScene.name == scene.SceneName)
                onLoad(loadedScene);
        };
    }

    public void GoToShop()
    {
        AudioManager.Instance.PlayMusic(ShopMusic);
        Debug.Log("Going to shop!");
        GameScene.ToolsShop.LoadSingle();
    }

    internal void GoToGameFinished()
    {
        AudioManager.Instance.PlayMusic(GameFinishedMusic);
        Debug.Log("Going to game finished!");
        GameScene.GameFinished.LoadSingle();
    }
}
