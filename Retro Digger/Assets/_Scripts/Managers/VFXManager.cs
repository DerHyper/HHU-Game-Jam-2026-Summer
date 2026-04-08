using Unity.VisualScripting;
using UnityEngine;

public class VFXManager : MonoBehaviour {
    public static VFXManager Instance { get; private set; }
    public float MicroExplosionEnergy = 1f;
    public int MicroExplosionFragmentCount = 5;
    public float SmallExplosionEnergy = 1f;
    public int SmallExplosionFragmentCount = 10;
    public float BigExplosionEnergy = 1f;
    public int BigExplosionFragmentCount = 50;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void CreateSmallExplosion(Vector2 position, Sprite[] fragments)
    {
        CreateExplosion(position, fragments, SmallExplosionEnergy, SmallExplosionFragmentCount);
    }

    public void CreateMicroExplosion(Vector2 position, Sprite[] fragments)
    {
        CreateExplosion(position, fragments, MicroExplosionEnergy, MicroExplosionFragmentCount);
    }

    public void CreateBigExplosion(Vector2 position, Sprite[] fragments)
    {
        CreateExplosion(position, fragments, BigExplosionEnergy, BigExplosionFragmentCount);
    }

    public void CreateExplosion(Vector2 position, Sprite[] fragments, float energy, int fragmentCount)
    {
        var explosion = new GameObject("Explosion");
        explosion.transform.position = position;
        explosion.transform.parent = gameObject.transform;
        explosion.AddComponent<Explosion>().Init(energy, fragments, fragmentCount);
    }

    public void CameraShake(float intensity, float duration)
    {
        // Implement camera shake logic here
    }
}