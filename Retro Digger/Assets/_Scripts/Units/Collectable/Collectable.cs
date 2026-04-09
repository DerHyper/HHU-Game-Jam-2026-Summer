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
    public AudioClip CollectSound;
    public AudioClip DamageSound;
    public AudioClip DestroySound;
    public Sprite[] DamageParticles;

    void Start()
    {
        UpdateSprite();
        UpdateCollider();
    }

    public abstract void Collect();
    public void DestroyGame()
    {
        AudioManager.Instance.PlayOnce(DestroySound);
        VFXManager.Instance.CreateBigExplosion(transform.position, DamageParticles);
        VFXManager.Instance.CreateBigExplosion(transform.position, DamageParticles);
        VFXManager.Instance.CreateBigExplosion(transform.position, DamageParticles);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        DiggingManager.Instance.EndDiggingLost();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        AudioManager.Instance.PlayOnce(DamageSound);
        VFXManager.Instance.CreateBigExplosion(transform.position, DamageParticles);
        CurrentHealth = math.clamp(CurrentHealth - damage, 0, MaxHealth);
        Debug.Log($"Collectable {Name} took {damage} damage, current health: {CurrentHealth}/{MaxHealth}");
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
        TakeDamage(InventoryManager.Instance.GetToolLevel());
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