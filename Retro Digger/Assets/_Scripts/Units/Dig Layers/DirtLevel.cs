using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirtLevel
{
    public int Level;
    public List<GameObject> DirtPrefabs;

    public GameObject GetRandom()
    {
        if (DirtPrefabs == null || DirtPrefabs.Count == 0)
        {
            return null;
        }
        return DirtPrefabs[Random.Range(0, DirtPrefabs.Count)];
    }
}