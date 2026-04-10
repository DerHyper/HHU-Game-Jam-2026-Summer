using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Videogame : Collectable
{
    public override void Collect()
    {
        AudioManager.Instance.PlayOnce(CollectSound);
        IsCollected = true;
        gameObject.transform.DOScale(1.2f, 0.5f)
        .SetEase(Ease.OutQuad)
        .SetLoops(4, LoopType.Yoyo)
        .OnComplete(() =>
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        });
        
    }
}