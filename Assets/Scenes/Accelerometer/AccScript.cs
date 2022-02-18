using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccScript : MonoBehaviour
{

    [SerializeField] Button btn;
    [SerializeField] Text pnl;

    public float timeRemaining = 10;

    public bool timeRunning = false;

    public bool shakeInTime = false;

    public float timestamp = 0;

    // Update is called once per frame
    void Update()
    {
        Vector3 acc = Input.acceleration;

        Debug.Log(timeRunning);

        if (timeRunning == true)
        {
            if (timeRemaining >= 0)
            {
                if (acc.sqrMagnitude >= 4F)
                {
                    btn.GetComponentInChildren<Text>().text = "Shake event detected at time " + Mathf.FloorToInt(Time.time);
                    shakeInTime = true;

                }
                else
                {
                    btn.GetComponentInChildren<Text>().text = "No Shake " + Mathf.FloorToInt(Time.time);
                }

                timeRemaining -= Time.deltaTime;
                pnl.text = "" + Mathf.FloorToInt(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timeRunning = false;
                pnl.text = "" + Mathf.FloorToInt(timeRemaining);
            }
        }
        else
        {
            Debug.Log("LOL");
        }

        if (shakeInTime == true)
        {
            Debug.Log("Send " + timestamp + " to host");
        }
    }

    public void ClickShakeButton()
    {
        timeRunning = true;
        timeRemaining = 10;
        shakeInTime = false;
    }
}
