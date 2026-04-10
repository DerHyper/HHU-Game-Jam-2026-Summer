using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;
using System.Diagnostics;
using DG.Tweening;

public class VFXManager : MonoBehaviour {
    public static VFXManager Instance { get; private set; }
    public float MicroExplosionEnergy = 1f;
    public int MicroExplosionFragmentCount = 5;
    public float SmallExplosionEnergy = 1f;
    public int SmallExplosionFragmentCount = 10;
    public float BigExplosionEnergy = 1f;
    public int BigExplosionFragmentCount = 50;
    public float CameraShakeIntensitySmall = 0.5f;
    public float CameraShakeIntensityMedium = 1f;
    public float CameraShakeIntensityBig = 5f;
    public float CameraShakeDurationSmall = 0.1f;
    public float CameraShakeDurationMedium = 0.2f;
    public float CameraShakeDurationBig = 0.5f;
    [SerializeField] private CinemachineBasicMultiChannelPerlin cameraNoise;

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
        CameraShake(CameraShakeIntensitySmall, CameraShakeDurationSmall);
    }

    public void CreateMicroExplosion(Vector2 position, Sprite[] fragments)
    {
        CreateExplosion(position, fragments, MicroExplosionEnergy, MicroExplosionFragmentCount);
    }

    public void CreateBigExplosion(Vector2 position, Sprite[] fragments)
    {
        CreateExplosion(position, fragments, BigExplosionEnergy, BigExplosionFragmentCount);
        CameraShake(CameraShakeIntensityBig, CameraShakeDurationBig);
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
        // Animate the camera noise amplitude to create a shake effect
        DOTween.To(
            () => cameraNoise.AmplitudeGain,      // Getter
            x => cameraNoise.AmplitudeGain = x,   // Setter
            intensity,                            // Goal
            duration                              // Duration
        )
        .SetEase(Ease.OutQuad)
        .SetLoops(2, LoopType.Yoyo)
        .OnComplete(() => cameraNoise.AmplitudeGain = 0); // Ensure it resets to 0 after shaking

        DOTween.To(
            () => cameraNoise.FrequencyGain,      // Getter
            x => cameraNoise.FrequencyGain = x,   // Setter
            intensity,                            // Goal
            duration                              // Duration
        )
        .SetEase(Ease.OutQuad)
        .SetLoops(2, LoopType.Yoyo)
        .OnComplete(() => cameraNoise.FrequencyGain = 0); // Ensure it resets to 0 after shaking
    }
}