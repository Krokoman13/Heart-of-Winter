using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupScript : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    Camera cam;

    bool onCooldown = false;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (canvas == null) canvas =  GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>();
    }

    public void SpawnPopup(string amount, Color color)
    {
        if (onCooldown) return;
        StartCoroutine(cooldown());

        Transform p = Instantiate(Resources.Load<Transform>("Popup"), canvas.transform);
        
        p.GetComponent<TMP_Text>().SetText(amount);
        p.GetComponent<TMP_Text>().faceColor = color;
        p.GetComponent<TMP_Text>().color = color;

        p.position = cam.WorldToScreenPoint(transform.position) - new Vector3(0, -260, 0);
    }

    public void SpawnPopup(string amount)
    {
        if (onCooldown) return;
        StartCoroutine(cooldown());

        Transform p = Instantiate(Resources.Load<Transform>("Popup"), canvas.transform);

        p.GetComponent<TMP_Text>().SetText(amount);

        p.position = cam.WorldToScreenPoint(transform.position) - new Vector3(0, -260, 0);
    }

    IEnumerator cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(1f);
        onCooldown = false;
    }
}
