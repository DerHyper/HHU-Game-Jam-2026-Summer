using UnityEngine;

public class Collider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered the collider.");
    }
}