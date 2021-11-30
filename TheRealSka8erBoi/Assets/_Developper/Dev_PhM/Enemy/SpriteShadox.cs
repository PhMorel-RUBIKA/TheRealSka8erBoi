using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadox : MonoBehaviour
{
    public Vector2 offset = new Vector2(-3, -3);

    public Material shadowMaterial;
    public Color ShadowColor;

    private SpriteRenderer _spriteCaster;
    private SpriteRenderer _spriteShadow;

    private Transform _transformCaster;
    private Transform _transformShadow;

    private void Start()
    {
        _transformCaster = transform;
        _transformShadow = new GameObject().transform;
        _transformShadow.parent = _transformCaster;
        _transformShadow.gameObject.name = "shadow";
        _transformShadow.localRotation = Quaternion.identity;

        _spriteCaster = GetComponent<SpriteRenderer>();
        _spriteShadow = _transformShadow.gameObject.AddComponent<SpriteRenderer>();

        _spriteShadow.material = shadowMaterial;
        _spriteShadow.color = ShadowColor;
        _spriteShadow.sortingLayerName = _spriteCaster.sortingLayerName;
    }

    private void LateUpdate()
    {
        _spriteShadow.sortingOrder = _spriteCaster.sortingOrder - 1;
        _transformShadow.position =
            new Vector2(_transformCaster.position.x + offset.x, _transformCaster.position.y + offset.y);
        _spriteShadow.sprite = _spriteCaster.sprite;
    }
}
