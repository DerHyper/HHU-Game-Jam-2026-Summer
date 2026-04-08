using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DiggingManager : MonoBehaviour {
    public static DiggingManager Instance { get; private set; }
    public int MaxDirtSpotsPerLevel = 3;
    [SerializeField] public Tool CurrentTool = null;
    [SerializeField] public List<Tool> CollectedTools = null;
    [SerializeField] public List<Dirt> CurrentDirtSpots = null;
    /// <summary>
    /// List of Dirt Prefab Lists, where each list corresponds to a dirt level. The first list is for level 1 dirt, the second for level 2, and so on.
    /// </summary>
    [SerializeField] public List<DirtLevel> DirtPrefabs;
    [SerializeField] private Collider2D DiggingArea;

    public void SpawnDirtSpots(int MaxDirtLevel = 1)
    {
        for (int level = 0; level < MaxDirtLevel && level < DirtPrefabs.Count; level++)
        {
            int spotsToSpawn = Random.Range(1, MaxDirtSpotsPerLevel + 1);
            for (int i = 0; i < spotsToSpawn; i++)
            {
                var dirtSpot = Instantiate(DirtPrefabs[level].GetRandom());
                dirtSpot.transform.parent = DiggingArea.transform;
                dirtSpot.transform.position = new Vector2(
                    Random.Range(DiggingArea.bounds.min.x, DiggingArea.bounds.max.x),
                    Random.Range(DiggingArea.bounds.min.y, DiggingArea.bounds.max.y)
                );
                var dirt = dirtSpot.GetComponent<Dirt>();
                CurrentDirtSpots.Add(dirt);
            }
        }
    }

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

    public void RemoveDiggingLayer(Dirt layer)
    {
        CurrentDirtSpots.Remove(layer);
        Destroy(layer.gameObject);
        if (CurrentDirtSpots.Count == 0)
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