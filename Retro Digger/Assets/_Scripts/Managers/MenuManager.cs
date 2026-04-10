using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> MenusToHideOnLeave = new();

    public List<GameObject> ScoreEntryContainers = new();

    /// <summary>
    /// Activates the given menu if it is inactive, and deactivates it if it is active.
    /// </summary>
    /// <param name="menu">Menu to switched</param>
    public void SwitchMenuVisibility(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
        return;
    }

    public void Start()
    {
        GameManager.Instance.WhenSceneUnloads(GameScene.MainMenu, (scene) =>
        {
            foreach (var menu in MenusToHideOnLeave)
            {
                menu.SetActive(false);
            }
        });

        GameManager.Instance.WhenSceneLoads(GameScene.MainMenu, (scene) =>
        {
            FillScoreboard();
        });

        FillScoreboard();
    }

    public async void FillScoreboard()
    {
        try
        {
            var scores = await ApiService.Instance.GetScores();
            for (int i = 0; i < ScoreEntryContainers.Count; i++)
            {
                var container = ScoreEntryContainers[i];
                if (i >= scores.Count)
                {
                    container.SetActive(false);
                    continue;
                }

                container.SetActive(true);
                var setter = container.GetComponent<ScoreEntrySetter>();
                setter.Set(scores[i]);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to load scores: " + ex.Message);
        }
    }
}
