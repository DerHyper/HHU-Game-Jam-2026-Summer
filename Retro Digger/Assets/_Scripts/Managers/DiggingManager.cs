using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DiggingManager : MonoBehaviour {
    public static DiggingManager Instance { get; private set; }
    [SerializeField] public Tool CurrentTool = null;
    [SerializeField] public List<Tool> CollectedTools = null;
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
}