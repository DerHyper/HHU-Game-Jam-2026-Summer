using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Collectable : MonoBehaviour, IPointerClickHandler
{
    public const int BaseSortingOrder = 0;
    public string Name;
    public Sprite Sprite;
    public int MaxValue;
    public int Level;
    public int MaxHealth = 10;
    public int CurrentHealth = 10;

    void Start()
    {
        UpdateSprite();
        UpdateCollider();
    }

    public abstract void Collect();
    public void DestroyGame()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth = math.clamp(CurrentHealth - damage, 0, MaxHealth);
        if (CurrentHealth <= 0)
        {
            DestroyGame();
        }
    }

    public int GetHealthPercentage()
    {
        return Mathf.RoundToInt((float)CurrentHealth / MaxHealth * 100);
    }

    public int GetCurrentValue()
    {
        return Mathf.RoundToInt(GetHealthPercentage() * MaxValue);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TakeDamage(InventoryManager.Instance.GetToolDamage() * InventoryManager.Instance.GetToolLevel());
    }

    private void UpdateCollider()
    {
        var collider = gameObject.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            return;
        }
        var polygonCollider = gameObject.GetComponent<PolygonCollider2D>();
        if (polygonCollider == null)
        {
            polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        }
        polygonCollider.CreateFromSprite(gameObject.GetComponent<SpriteRenderer>().sprite);
    }
    
    private void UpdateSprite()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = Sprite;
        spriteRenderer.sortingOrder = BaseSortingOrder;
    }
}