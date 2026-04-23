using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Videogame : Collectable
{
    public override async Task Collect()
    {
        AudioManager.Instance.PlayOnce(CollectSound);
        IsCollected = true;
        
        await gameObject.transform.DOScale(1.2f, 0.5f)
        .SetEase(Ease.OutQuad)
        .SetLoops(4, LoopType.Yoyo)
        .OnComplete(() =>
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }).AsyncWaitForCompletion();
        
    }
}