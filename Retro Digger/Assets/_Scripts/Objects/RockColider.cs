using DG.Tweening;
using UnityEngine;

public class RockCollider : MonoBehaviour
{
    public void HitByPlayer(PlayerController player)
    {
        Debug.Log("Player entered the collider.");
        GameManager
            .Instance
            .GoToDiggingView();
    }

    public void DestroyRock()
    {
        float destroyDuration = 0.5f;
        transform
        .DOScale(0, destroyDuration)
        .SetEase(Ease.InBack)
        .OnComplete(() => {
            Destroy(gameObject);
            });
    }
}