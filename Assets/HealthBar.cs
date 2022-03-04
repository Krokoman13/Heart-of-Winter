using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] float value = 1.0f;

    [SerializeField] GameObject bar;
    SpriteRenderer barImage;

    Text text;
    Camera cam;

    float spriteWidth;

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        GameObject textObject = new GameObject();
        textObject.transform.parent = GameObject.FindGameObjectWithTag("UI").transform;
        text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

        barImage = bar.GetComponent<SpriteRenderer>();
        spriteWidth = barImage.sprite.rect.width/100.0f;
    }

    public void SetValue(float minHealth, float maxHealth)
    {
        barImage.color = Color.red;
        text.text = Mathf.Round(minHealth).ToString() + '/' + maxHealth.ToString();
        setValue(minHealth / maxHealth);
    }

    private void setValue(float pValue)
    {
        value = Mathf.Clamp(pValue, 0.0f, 1.0f);
        bar.transform.localScale = new Vector3 (value, 1, 1);

        float offSetX = (spriteWidth / 2) * (value - 1);

        bar.transform.localPosition = new Vector3(offSetX, 0, 0);
    }

    private void Update()
    {
        text.transform.position = cam.WorldToScreenPoint(transform.position) - new Vector3(-75, 105, 0);
    }

    private void OnDestroy()
    {
        Destroy(text);
    }

    public void Shielded(float amount)
    {
        text.text = "    " + amount.ToString();
        barImage.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }
    /*
   private void OnValidate()
   {
       setValue(value);
   }*/
}
