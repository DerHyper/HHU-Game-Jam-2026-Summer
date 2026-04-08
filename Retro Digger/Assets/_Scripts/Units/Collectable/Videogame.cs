using Unity.VisualScripting;
using UnityEngine;

public class Videogame : Collectable 
{
    public override void Collect()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        UpdateSprite();
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