using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> MenusToHideOnLeave = new();

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
    }
}
