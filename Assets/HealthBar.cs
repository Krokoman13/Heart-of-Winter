using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] float value = 1.0f;

    [SerializeField] GameObject bar;

    float spriteWidth;

    private void Awake()
    {
        spriteWidth = bar.GetComponent<SpriteRenderer>().sprite.rect.width/100.0f;
    }

    public void setValue(float pValue)
    {
        value = Mathf.Clamp(pValue, 0.0f, 1.0f);
        bar.transform.localScale = new Vector3 (value, 1, 1);

        float offSetX = (spriteWidth / 2) * (value - 1);

        Debug.Log(offSetX);
        bar.transform.localPosition = new Vector3(offSetX, 0, 0);
    }
/*
    private void OnValidate()
    {
        setValue(value);
    }*/
}
