using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_Text statusText;
    [SerializeField] private int noOfPlayers = 2;

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createInput.text))
        {
            ShowError("Room name cannot be empty!");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = noOfPlayers; // Set player limit
        PhotonNetwork.CreateRoom(createInput.text, roomOptions);

    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinInput.text))
        {
            ShowError("Room name cannot be empty!");
            return;
        }

        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        // Clear any previous errors
        statusText.text = "";

        // Check if room is full
        if (PhotonNetwork.CurrentRoom.PlayerCount > 2)
        {
            ShowError("Room is full! (2/2 players)");
            PhotonNetwork.LeaveRoom();
            return;
        }
        PhotonNetwork.LoadLevel("MainMenu");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ShowError($"Failed to join room: {message}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ShowError($"Failed to create room: {message}");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player joined: {newPlayer.NickName}");
    }

    private void ShowError(string message)
    {
        statusText.text = message;
        StartCoroutine(ClearErrorAfterDelay(2f));
    }
    private IEnumerator ClearErrorAfterDelay(float seconds)
{
    yield return new WaitForSeconds(seconds);
    statusText.text = "";
}
}
