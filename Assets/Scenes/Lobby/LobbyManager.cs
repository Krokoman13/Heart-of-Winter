using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

using HeartOfWinter.PlayerInformation;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject findMatchBtn;

    [SerializeField]
    GameObject searchingPanel;

    [SerializeField]
    GameObject hostingBtn;

    [SerializeField]
    InputField inputField;

    private void Awake()
    {
        searchingPanel.SetActive(false);
        findMatchBtn.SetActive(false);
        hostingBtn.SetActive(false);

        PhotonNetwork.ConnectUsingSettings();
        //DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Established conntection to Photon: " + PhotonNetwork.CloudRegion + "Server");
        //PhotonNetwork.AutomaticallySyncScene = true;

        findMatchBtn.SetActive(true);
        hostingBtn.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Could not find Room: "+inputField.text);
        //Debug.Log("Trying to join a random room instead");
        //PhotonNetwork.JoinRandomRoom();

        StopSearch();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Could not find Room");

        StopSearch();
    }

    public void MakeRoom()
    {
        string roomName = inputField.text;

        if (roomName.Length < 2)
        { 
            roomName = Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString();
        }

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 3 };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Created room: " + roomName + ". Waiting for another player.");
    }

    public override void OnJoinedRoom()
    {
        PlayerInfo.ID = PhotonNetwork.PlayerList.Length;
        /*        Debug.Log("Player: " + PlayerInfo.ID);
                Destroy(gameObject);*/

        PhotonNetwork.AutomaticallySyncScene = true;

        SceneManager.LoadScene(1);
    }

    public override void OnCreatedRoom()
    {
        PlayerInfo.ID = 1;
        SceneManager.LoadScene(1);
    }

    public void FindMatch()
    {
        searchingPanel.SetActive(true);
        findMatchBtn.SetActive(false);
        hostingBtn.SetActive(false);

        Debug.Log("Searching for a Game");

        if (inputField.text == "")
        {
            PhotonNetwork.JoinRandomRoom();
            return;
        }


        //Try to join a room
        PhotonNetwork.JoinRoom(inputField.text);
    }

    public void StopSearch()
    {
        searchingPanel.SetActive(false);
        findMatchBtn.SetActive(true);
        hostingBtn.SetActive(true);

        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
        Debug.Log("Stopped Connecting");
    }
}
