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
}