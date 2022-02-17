using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccScript : MonoBehaviour
{

    [SerializeField] Button btn;
    [SerializeField] Text pnl;

    public float timeRemaining = 10;

    public bool timeRunning = true;

    public bool shakeInTime = false;


    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.acceleration);
        Vector3 acc = Input.acceleration;



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
    }
}
