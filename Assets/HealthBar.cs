using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] float value = 1.0f;

    [SerializeField] GameObject bar;

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

        spriteWidth = bar.GetComponent<SpriteRenderer>().sprite.rect.width/100.0f;
    }

    public void SetValue(float minHealth, float maxHealth)
    {
        text.text = minHealth.ToString() + '/' + maxHealth.ToString();
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
        text.transform.position = cam.WorldToScreenPoint(transform.position) - new Vector3(-30, 40, 0);
    }

    private void OnDestroy()
    {
        Destroy(text);
    }
    /*
        private void OnValidate()
        {
            setValue(value);
        }*/
}
