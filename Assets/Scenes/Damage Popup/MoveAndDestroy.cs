using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveAndDestroy : MonoBehaviour
{

    private float disappearTimer = 1f;
    private Color colour;

    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetComponent<TMP_Text>();
        colour = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(5, 20) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            colour.a -= 2f * Time.deltaTime;
            text.color = colour;
        }
        if (colour.a < 0.0)
        {
            Destroy(gameObject);
        }
    }
}
