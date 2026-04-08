using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DiggingManager : MonoBehaviour {
    public static DiggingManager Instance { get; private set; }
    [SerializeField] public Tool CurrentTool = null;
    [SerializeField] public List<Tool> CollectedTools = null;
    [SerializeField] public List<DiggingLayer> DiggingLayers = null;
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

    public void RemoveDiggingLayer(DiggingLayer layer)
    {
        DiggingLayers.Remove(layer);
        Destroy(layer.gameObject);
        if (DiggingLayers.Count == 0)
        {
            Debug.Log("All digging layers removed!");
            EndDigging();
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void EndDigging()
    {
        throw new System.NotImplementedException("TODO");
    }
}