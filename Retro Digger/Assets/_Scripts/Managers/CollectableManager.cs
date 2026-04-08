using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectableManager : MonoBehaviour {
    public static CollectableManager Instance { get; private set; }
    public List<Collectable> Collectables;
    public Transform CollectablePosition;
    public Collectable CurrentCollectable;

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

    public void SpawnCollectable(int level)
    {
        CurrentCollectable = Instantiate(GetRandomLevelCollectable(level), CollectablePosition);
        CurrentCollectable.transform.parent = CollectablePosition;
        CurrentCollectable.transform.position = CollectablePosition.position;
        CurrentCollectable.AddComponent<SpriteRenderer>().sprite = CurrentCollectable.Sprite;
    }


    public List<Collectable> GetLevelCollectables(int level)
    {
        if (Collectables.Count == 0) return null;
        List<Collectable> levelCollectables = Collectables.FindAll(c => c.Level == level);
        return levelCollectables;
    }

    public Collectable GetRandomLevelCollectable(int level)
    {
        if (Collectables.Count == 0) return null;
        List<Collectable> levelCollectables = Collectables.FindAll(c => c.Level == level);

        return levelCollectables[Random.Range(0, levelCollectables.Count)];
    }
    
    public List<Collectable> GetCollectables()
    {
        if (Collectables.Count == 0) return null;
        return Collectables;
    }

    public Collectable GetRandomCollectable()
    {
        if (Collectables.Count == 0) return null;
        return Collectables[Random.Range(0, Collectables.Count)];
    }
}