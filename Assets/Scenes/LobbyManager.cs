using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject findMatchBtn;

    [SerializeField]
    GameObject searchingPanel;

    private void Start()
    {
        searchingPanel.SetActive(false);
        findMatchBtn.SetActive(false);

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Established conntection to Photon: " + PhotonNetwork.CloudRegion + "Server");
        PhotonNetwork.AutomaticallySyncScene = true;

        findMatchBtn.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Could not find a Room - Creating a Room");
        MakeRoom();
    }

    void MakeRoom()
    {
        int randomRoomint = Random.Range(0, 5000);
        string randomRoomName = "RoomName_" + randomRoomint;

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
        Debug.Log("Created room: " + randomRoomName + ". Wating for another player.");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + "/2 Starting Game");
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void FindMatch()
    {
        searchingPanel.SetActive(true);
        findMatchBtn.SetActive(false);

        //Try to join a room
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Searching for a Game");
    }

    public void StopSearch()
    {
        searchingPanel.SetActive(false);
        findMatchBtn.SetActive(true);

        PhotonNetwork.LeaveRoom();
        Debug.Log("Stopped Connecting");
    }
}
