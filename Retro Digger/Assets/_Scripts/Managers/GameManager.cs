using System;
using UnityEngine;
using UnityEngine.Events;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(GameScene.MainMenu.SceneName, LoadSceneMode.Single);
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
        if (SceneManager.GetActiveScene().name == GameScene.DiggingView.SceneName)
        {
            SceneManager.UnloadSceneAsync(GameScene.DiggingView.SceneName).completed += (op) =>
            {
                SceneManager.LoadScene(GameScene.MapView.SceneName, LoadSceneMode.Single);
            };
            return;
        }

        SceneManager.LoadScene(GameScene.MapView.SceneName, LoadSceneMode.Single);
    }

    public void GoToDiggingView()
    {
        if (SceneManager.GetActiveScene().name == GameScene.MapView.SceneName)
        {
            SceneManager.LoadScene(GameScene.DiggingView.SceneName, LoadSceneMode.Additive);
            return;
        }

        SceneManager.LoadScene(GameScene.DiggingView.SceneName, LoadSceneMode.Single);
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
}
