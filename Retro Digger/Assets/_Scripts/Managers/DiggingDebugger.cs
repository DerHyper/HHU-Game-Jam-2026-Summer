using UnityEngine;

public class DiggingDebugger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DiggingManager.Instance.SpawnDirtSpots();
    }
}
