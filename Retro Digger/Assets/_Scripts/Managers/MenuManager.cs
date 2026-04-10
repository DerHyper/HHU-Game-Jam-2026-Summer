using UnityEngine;

public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Activates the given menu if it is inactive, and deactivates it if it is active.
    /// </summary>
    /// <param name="menu">Menu to switched</param>
    public void SwitchMenuVisibility(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
        return;
    }
}
