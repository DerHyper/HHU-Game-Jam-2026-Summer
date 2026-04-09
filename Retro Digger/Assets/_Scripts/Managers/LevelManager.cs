using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public sealed class LevelManager : MonoBehaviour
{
    [Serializable]
    public class LevelData
    {
        public string levelName;
        public List<GameObject> objectsToActivate = new();
    }

    [Header("Configured in Inspector")]
    [SerializeField] private List<LevelData> levels = new();

    [Header("Startup")]
    [SerializeField] private int startLevelIndex = 0;
    [SerializeField] private bool loadOnStart = true;

    [Header("Events")]
    public UnityEvent<int> onLevelLoaded;

    public int CurrentLevelIndex { get; private set; } = -1;
    public int LevelCount => levels.Count;

    private readonly HashSet<GameObject> _allManagedObjects = new();

    private void Awake()
    {
        RebuildManagedObjectCache();
    }

    private void Start()
    {
        if (loadOnStart && levels.Count > 0)
            LoadLevel(startLevelIndex);
    }

    public void LoadLevel(int index)
    {
        if (levels is { Count: 0 })
        {
            Debug.LogWarning("LevelManager: No levels configured.");
            return;
        }

        if (index < 0 || index >= levels.Count)
        {
            Debug.LogWarning($"LevelManager: Invalid level index {index}. Valid range is 0..{levels.Count - 1}");
            return;
        }

        LevelData target = levels[index];

        foreach (var obj in _allManagedObjects.NotNull())
            obj.SetActive(false);

        foreach (var obj in target.objectsToActivate.NotNull())
            obj.SetActive(true);

        CurrentLevelIndex = index;
        onLevelLoaded?.Invoke(CurrentLevelIndex);
    }

    public void NextLevel()
    {
        if (levels is { Count: 0 }) return;
        LoadLevel((CurrentLevelIndex + 1) % levels.Count);
    }

    public void PreviousLevel()
    {
        if (levels is { Count: 0 }) return;
        LoadLevel((CurrentLevelIndex - 1 + levels.Count) % levels.Count);
    }

    // UI-friendly: Button can call this with an int parameter.
    public void LoadLevelFromUI(int index) => LoadLevel(index);

    // UI-friendly: add a new level from a root object (all children become this level's objects).
    public void AddLevelFromRoot(GameObject levelRoot)
    {
        if (levelRoot == null)
        {
            Debug.LogWarning("LevelManager: AddLevelFromRoot got null root.");
            return;
        }

        LevelData newLevel = new() { levelName = levelRoot.name };

        foreach (Transform c in levelRoot.transform)
        {
            if (c != null)
                newLevel.objectsToActivate.Add(c.gameObject);
        }

        levels.Add(newLevel);
        RebuildManagedObjectCache();
    }

    // UI-friendly: create an empty slot first, then fill later in Inspector/code.
    public void AddEmptyLevel()
    {
        levels.Add(new LevelData { levelName = $"Level {levels.Count}" });
    }

    public void RemoveLevel(int index)
    {
        if (index < 0 || index >= levels.Count) return;

        levels.RemoveAt(index);
        RebuildManagedObjectCache();

        if (levels is { Count: 0 })
        {
            CurrentLevelIndex = -1;
            return;
        }

        if (CurrentLevelIndex >= levels.Count)
            CurrentLevelIndex = levels.Count - 1;
    }

    private void RebuildManagedObjectCache()
    {
        _allManagedObjects.Clear();

        foreach (LevelData level in levels.NotNull())
        {
            _allManagedObjects.AddRange(
                level.objectsToActivate.NotNull());
        }
    }
}