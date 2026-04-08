using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _fragments;
    [SerializeField]
    private float _energy;
    [SerializeField]
    private float _lifetime = 2f;
    [SerializeField]
    private float _fragmentCount = 2f;
    [SerializeField]
    private int _spriteLayer = 10;
    private float _explosionSize = 0.4f;

    public void Init(float energy, Sprite[] fragments, int fragmentCount)
    {
        _energy = energy;
        _fragments = fragments;
        _fragmentCount = fragmentCount;
        DoExplosion();
    }

    private void DoExplosion()
    {
        Vector2 currentPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        foreach (Sprite fragment in GetRandomFragments())
        {
            Vector2 distanceFromCenter = UnityEngine.Random.insideUnitCircle;
            Vector2 randomStartPosition = currentPosition + distanceFromCenter * _explosionSize;

            // Instantiate new GameObject
            GameObject instantiatedFragment = new GameObject("ExplosionFragment");
            instantiatedFragment.transform.position = randomStartPosition;
            instantiatedFragment.transform.parent = gameObject.transform;
            SpriteRenderer spriteRenderer = instantiatedFragment.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = fragment;
            spriteRenderer.sortingOrder = _spriteLayer;
            instantiatedFragment.AddComponent<Rigidbody2D>().AddForce(distanceFromCenter * _energy);
            instantiatedFragment.AddComponent<ExplosionFragment>();
        }
        Invoke(nameof(SelfDestroy), _lifetime);
    }

    private List<Sprite> GetRandomFragments()
    {
        List<Sprite> fragments = new List<Sprite>();
        for (int i = 0; i < _fragmentCount; i++)
        {
            fragments.Add(_fragments[UnityEngine.Random.Range(0, _fragments.Length)]);
        }
        return fragments;
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}