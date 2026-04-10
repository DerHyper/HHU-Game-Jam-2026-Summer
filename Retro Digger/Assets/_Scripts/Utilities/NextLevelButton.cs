using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        GameManager.Instance.NextDay();
    }
}
