using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class RockVariantSelector : MonoBehaviour
{
    [System.Serializable]
    private class VariantEntry
    {
        public GameObject prefab;
        public Vector3 localPositionOffset;
        public Vector3 localEulerOffset;
        public Vector3 localScale = Vector3.one;
    }

    [SerializeField] private List<VariantEntry> variants = new();
    [SerializeField] private int selectedIndex = 0;

    [SerializeField, HideInInspector] private Transform variantContainer;
    [SerializeField, HideInInspector] private GameObject activeInstance;

    private bool isApplying;

    private void OnEnable()
    {
        ApplyVariant();
    }

    private void Awake()
    {
        ApplyVariant();
    }

    private void OnValidate()
    {
        selectedIndex = Mathf.Max(0, selectedIndex);
        ApplyVariant();
    }

    public void SetVariant(int index)
    {
        selectedIndex = index;
        ApplyVariant();
    }

    private void ApplyVariant()
    {
        if (isApplying) return;
        isApplying = true;

        EnsureContainer();
        ClearContainer();

        if (variants == null || variants.Count == 0)
        {
            activeInstance = null;
            isApplying = false;
            return;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, variants.Count - 1);
        VariantEntry selected = variants[selectedIndex];

        if (selected.prefab == null)
        {
            activeInstance = null;
            isApplying = false;
            return;
        }

        activeInstance = CreateInstance(selected.prefab);
        if (activeInstance != null)
        {
            ApplyOffsets(activeInstance.transform, selected);
        }

        isApplying = false;
    }

    private void EnsureContainer()
    {
        if (variantContainer != null) return;

        Transform existing = transform.Find("_VariantContainer");
        if (existing != null)
        {
            variantContainer = existing;
            return;
        }

        GameObject go = new("_VariantContainer");
        go.transform.SetParent(transform, false);
        variantContainer = go.transform;
    }

    private void ClearContainer()
    {
        if (variantContainer == null) return;

        for (int i = variantContainer.childCount - 1; i >= 0; i--)
        {
            GameObject child = variantContainer.GetChild(i).gameObject;

            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }

        activeInstance = null;
    }

    private GameObject CreateInstance(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, variantContainer);
            instance.name = prefab.name;
            return instance;
        }
#endif

        return Instantiate(prefab, variantContainer);
    }

    private static void ApplyOffsets(Transform t, VariantEntry v)
    {
        t.localPosition = v.localPositionOffset;
        t.localRotation = Quaternion.Euler(v.localEulerOffset);
        t.localScale = v.localScale;
    }
}