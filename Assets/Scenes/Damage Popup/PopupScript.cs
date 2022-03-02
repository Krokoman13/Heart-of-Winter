using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupScript : MonoBehaviour
{

    [SerializeField] private TMP_Text popup;

    public void SpawnPopup(float amount)
    {
        popup.SetText(amount.ToString());

        Transform p = Instantiate(popup.transform, transform.parent);
        Vector3 t = new Vector3(50, 50, 0);
        p.localPosition = transform.localPosition + t;
    }
}
