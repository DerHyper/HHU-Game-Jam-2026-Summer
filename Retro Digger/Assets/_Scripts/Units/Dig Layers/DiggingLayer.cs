using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiggingLayer : MonoBehaviour, IPointerClickHandler
{
    public int HealthAverage = 10;
    public int HealthDeviation = 1;
    public int CurrentMaxHealth = 10;
    public int CurrentHealth = 10;
    public int ToolLevelRequired = 1;
    public Sprite[] Particles;

    private void Start()
    {
        UpdateCollider();
        SetHealth();
    }

    private void SetHealth()
    {
        CurrentMaxHealth = HealthAverage + UnityEngine.Random.Range(-HealthDeviation, HealthDeviation + 1);
        CurrentHealth = CurrentMaxHealth;
    }

    private void UpdateCollider()
    {
        var polygonCollider = gameObject.GetComponent<PolygonCollider2D>();
        if (polygonCollider == null)
        {
            polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        }
        polygonCollider.CreateFromSprite(gameObject.GetComponent<SpriteRenderer>().sprite);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("DiggingLayer clicked!");
        CheckDamage();
        CheckDestroy();
    }

    private void CheckDestroy()
    {
        if (CurrentHealth > 0) return;
        
        Debug.Log("Layer dug through!");
        DiggingManager.Instance.RemoveDiggingLayer(this);
    }

    private void CheckDamage()
    {
        var toolLevel = DiggingManager.Instance.GetToolLevel();
        if (toolLevel < ToolLevelRequired)
        {
            Debug.Log("Tool level too low to dig this layer!");
            VFXManager.Instance.CreateMicroExplosion(InputManager.Instance.GetMouseWorldPosition(), Particles);
            return;
        }
        else if (toolLevel > ToolLevelRequired)
        {
            Debug.Log("Tool level too high to dig this layer!");
            VFXManager.Instance.CreateBigExplosion(InputManager.Instance.GetMouseWorldPosition(), Particles);
            return;
        }
        else
        {
            DoDamage();
            VFXManager.Instance.CreateSmallExplosion(InputManager.Instance.GetMouseWorldPosition(), Particles);
        }
    }

    private void DoDamage()
    {
        var damage = DiggingManager.Instance.GetToolDamage();
        CurrentHealth = math.clamp(CurrentHealth - damage, 0, CurrentMaxHealth);
        Debug.Log($"Dealt {damage} damage to the layer. Current health: {CurrentHealth}/{CurrentMaxHealth}");
    }
}
