using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCycler : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;

    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetImage(int i)
    {
        if (i >= sprites.Count) return;
        spriteRenderer.sprite = sprites[i];
    }
}
