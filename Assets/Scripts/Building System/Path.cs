using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : Building
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Paths");
    }


    public void setSprite(int index)
    {
        spriteRenderer.sprite = sprites[index];
    }





}
