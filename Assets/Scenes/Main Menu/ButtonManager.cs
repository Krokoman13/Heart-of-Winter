using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.SceneManagement;

using Photon.Pun;

public class ButtonManager : MonoBehaviour
{
    public void ButtonMoveScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }
} 