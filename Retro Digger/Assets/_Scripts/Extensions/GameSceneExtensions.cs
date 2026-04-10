using System;
using UnityEngine.SceneManagement;

public static class GameSceneExtensions
{
    public static bool IsLatestScene(this GameScene self) 
        => SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name == self.SceneName;

    public static void LoadSingle(this GameScene self)
        => SceneManager.LoadScene(self.ScenePath, LoadSceneMode.Single);

    public static void LoadAdditive(this GameScene self)
        => SceneManager.LoadScene(self.ScenePath, LoadSceneMode.Additive);
}