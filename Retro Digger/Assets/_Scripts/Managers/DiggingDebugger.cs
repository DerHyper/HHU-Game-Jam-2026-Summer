using UnityEngine;

public class DiggingDebugger : MonoBehaviour
{
    public int MaxDirtLevel = 1;
    public int LootLevel = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CollectableManager.Instance.SpawnCollectable(LootLevel);
        DiggingManager.Instance.SpawnDirtSpots(MaxDirtLevel);
        InventoryManager.Instance.UpdateInventoryUI();
    }
}
