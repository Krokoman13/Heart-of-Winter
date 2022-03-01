using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.1f;

    RectTransform rectTransform;

    bool toTheRight = true;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = rectTransform.localPosition;

        if (toTheRight)
        {
            newPos.x -= scrollSpeed * Time.deltaTime;
            if (newPos.x < -1160) toTheRight = false;
        }
        else 
        {
            newPos.x += scrollSpeed * Time.deltaTime;
            if (newPos.x > -5) toTheRight = true;
        }

        rectTransform.localPosition = newPos;
    }
}
