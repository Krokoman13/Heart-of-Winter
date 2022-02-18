using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using HeartOfWinter.PlayerInformation;

public class SetupUIInfo : MonoBehaviourPun
{
    [SerializeField]
    GameObject roomIDTextBox;

    [SerializeField]
    GameObject playerIDTextBox;

    Text roomIDText;
    Text playerIDText;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected) return;
        playerIDText = playerIDTextBox.GetComponent<Text>();
        playerIDText.text = PlayerInfo.name;

        roomIDText = roomIDTextBox.GetComponent<Text>();

        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC(nameof(SetLobbyName), RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.Name);
    }

    [PunRPC]
    void SetLobbyName(string s)
    {
        roomIDText.text = "Room ID: " + s;
    }
}
