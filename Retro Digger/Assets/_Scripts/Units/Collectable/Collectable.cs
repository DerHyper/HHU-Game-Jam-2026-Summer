using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public const int BaseSortingOrder = 0;
    public string Name;
    public Sprite Sprite;
    public int Value;
    public int Level;

    public abstract void Collect();
}