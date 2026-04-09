using System.Collections.Generic;
using UnityEngine;

public class RockVariantSelector : MonoBehaviour
{
    [System.Serializable]
    private class VariantEntry
    {
        public GameObject prefab;              // prefab to instantiate
        public Vector3 localPositionOffset;    // per skin offset
        public Vector3 localEulerOffset;       // optional rotation offset
        public Vector3 localScale = Vector3.one;
    }

    [SerializeField] private List<VariantEntry> variants = new();
    [SerializeField] private int selectedIndex = 0;

    private GameObject activeInstance;
    private int activeIndex = -1;

    private void Awake()
    {
        ApplyVariant();
    }

    private void OnValidate()
    {
        selectedIndex = Mathf.Max(0, selectedIndex);
        if (Application.isPlaying)
            ApplyVariant();
    }

    public void SetVariant(int index)
    {
        selectedIndex = index;
        ApplyVariant();
    }

    private void ApplyVariant()
    {
        if (variants == null || variants.Count == 0) return;

        selectedIndex = Mathf.Clamp(selectedIndex, 0, variants.Count - 1);
        VariantEntry selected = variants[selectedIndex];
        if (selected.prefab == null) return;

        // Reuse if same variant already active.
        if (activeInstance != null && activeIndex == selectedIndex)
        {
            ApplyOffsets(activeInstance.transform, selected);
            return;
        }

        if (activeInstance != null)
            Destroy(activeInstance);

        activeInstance = Instantiate(selected.prefab, transform);
        activeIndex = selectedIndex;

        ApplyOffsets(activeInstance.transform, selected);
    }

    private static void ApplyOffsets(Transform t, VariantEntry v)
    {
        t.localPosition = v.localPositionOffset;
        t.localRotation = Quaternion.Euler(v.localEulerOffset);
        t.localScale = v.localScale;
    }
}