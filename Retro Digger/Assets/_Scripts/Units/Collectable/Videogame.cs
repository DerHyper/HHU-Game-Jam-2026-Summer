using Unity.VisualScripting;
using UnityEngine;

public class Videogame : Collectable
{
    public override void Collect()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}