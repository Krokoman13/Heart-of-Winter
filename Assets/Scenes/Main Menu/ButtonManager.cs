using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.SceneManagement;

using Photon.Pun;
using System;

public class ButtonManager : MonoBehaviour
{
    public void ButtonMoveScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    public void ToMainMenu()
    {
        if (PhotonNetwork.InRoom)
        {
            StartCoroutine(disconnect());
            return;
        }
        else if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();

        SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator disconnect()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected) yield return 0;

        SceneManager.LoadScene("Main Menu");
    }
}