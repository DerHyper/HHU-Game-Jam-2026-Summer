using Unity.VisualScripting;
using UnityEngine;

public class Videogame : Collectable
{
    public override void Collect()
    {
        AudioManager.Instance.PlayOnce(CollectSound);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}