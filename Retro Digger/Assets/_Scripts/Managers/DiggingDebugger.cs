using UnityEngine;

public class DiggingDebugger : MonoBehaviour
{
    public int MaxDirtLevel = 1;
    public int LootLevel = 1;
    public bool DoDebug = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!DoDebug)
        {
            return;
        }
        ToolService.Instance.BuyFreeTool("Chisel");
        ToolService.Instance.BuyFreeTool("Hammer");

        CollectableManager.Instance.SpawnCollectable(LootLevel);
        DiggingManager.Instance.SpawnDirtSpots(MaxDirtLevel);
        InventoryManager.Instance.UpdateInventoryUI();
        TimeManager.Instance.StartDay();
    }
}
