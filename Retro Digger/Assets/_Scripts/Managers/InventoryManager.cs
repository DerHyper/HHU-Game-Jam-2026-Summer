using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    [SerializeField] public Tool CurrentTool = null;
    [SerializeField] public List<Tool> CollectedTools = null;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject inventoryElementPrefab;
    [SerializeField] private List<Collectable> colletedCollectables = new();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateInventoryUI()
    {
        //Delete old
        for (int i = 0; i < inventoryUI.transform.childCount; i++)
        {
            Destroy(inventoryUI.transform.GetChild(i).gameObject);
        }
        
        // Generate new
        CollectedTools.Sort((a, b) => a.UiOrder.CompareTo(b.UiOrder));
        for (int i = 0; i < CollectedTools.Count; i++)
        {
            var tool = CollectedTools[i];
            GameObject uiElement = Instantiate(inventoryElementPrefab, inventoryUI.transform);
            uiElement.GetComponent<ToolButton>().UpdateUI(tool);
        }
    }

    public int GetToolDamage()
    {
        if (CurrentTool == null)
        {
            return 0;
        }
        return CurrentTool.DiggingDamager;
    }

    public int GetToolLevel()
    {
        if (CurrentTool == null)
        {
            return 0;
        }
        return CurrentTool.Level;
    }

    public void CollectCollectable(Collectable collectable)
    {
        colletedCollectables.Add(collectable);
        collectable.Collect();
    }

    public void CollectTool(Tool tool)
    {
        CollectedTools.Add(tool);
        UpdateInventoryUI();
    }
}
