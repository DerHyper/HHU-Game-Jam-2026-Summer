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

        GoToMap();
    }

    public void GoToMap()
    {
        if (!GameScene.DiggingView.IsLatestScene())
        {
            Debug.Log("Replacing with map view");
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
            return;
        }

        Debug.Log("Replacing with digging view");
        GameScene.DiggingView.LoadSingle();
    }

    public void EndGame()
    {
        Debug.Log("Game Ended!");
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
}
