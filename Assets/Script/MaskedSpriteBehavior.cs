using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedSpriteBehavior : MonoBehaviour
{
    SpriteMask spriteMask;
    SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteMask = GetComponent<SpriteMask>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteMask.sprite = spriteRenderer.sprite;
    }
}
