using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiggingManager : MonoBehaviour
{
    public static DiggingManager Instance { get; private set; }
    public int MaxDirtSpotsPerLevel = 3;
    [SerializeField] public List<Dirt> CurrentDirtSpots = null;
    /// <summary>
    /// List of Dirt Prefab Lists, where each list corresponds to a dirt level. The first list is for level 1 dirt, the second for level 2, and so on.
    /// </summary>
    [SerializeField] public List<DirtLevel> DirtPrefabs;
    [SerializeField] public Transform DirtParent;

    public void SpawnDirtSpots(int MaxDirtLevel = 1)
    {
        int spriteLayer = 1;
        for (int level = 0; level < MaxDirtLevel && level < DirtPrefabs.Count; level++)
        {
            int spotsToSpawn = Random.Range(1, MaxDirtSpotsPerLevel + 1);

            for (int i = 0; i < spotsToSpawn; i++)
            {
                var dirtSpot = Instantiate(DirtPrefabs[level].GetRandom());
                var diggingArea = CollectableManager.Instance.GetCollectableArea();
                dirtSpot.transform.parent = DirtParent;
                dirtSpot.GetComponent<SpriteRenderer>().sortingOrder = spriteLayer;
                dirtSpot.transform.position = new Vector2(
                    Random.Range(diggingArea.bounds.min.x, diggingArea.bounds.max.x),
                    Random.Range(diggingArea.bounds.min.y, diggingArea.bounds.max.y)
                );
                var dirt = dirtSpot.GetComponent<Dirt>();
                CurrentDirtSpots.Add(dirt);
                spriteLayer++;
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

    public void RemoveDiggingLayer(Dirt layer)
    {
        CurrentDirtSpots.Remove(layer);
        Destroy(layer.gameObject);
        if (CurrentDirtSpots.Count == 0)
        {
            Debug.Log("All digging layers removed!");
            EndDiggingWon();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void EndDiggingWon()
    {
        Collectable currentCollectable = CollectableManager.Instance.CurrentCollectable;

        InventoryManager.Instance.CollectCollectable(currentCollectable);
        MoneyManager.Instance.AddMoneyAndScore(currentCollectable.GetCurrentValue());

        throw new System.NotImplementedException("TODO");
    }

    /// <summary>
    /// 
    /// </summary>
    public void EndDiggingLost()
    {
        throw new System.NotImplementedException("TODO");
    }
}