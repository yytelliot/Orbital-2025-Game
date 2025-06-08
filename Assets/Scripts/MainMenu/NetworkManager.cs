using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
[RequireComponent(typeof(PhotonView))]
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerCountText;
    
    private void Start()
    {
        UpdatePlayerCount();
    }

    // Automatically called when any player joins/leaves
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();
    }

    private void UpdatePlayerCount()
    {
        if (!PhotonNetwork.InRoom) return;
        
        playerCountText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount}/2 Players";
        
        // Optional: Auto-start if 2 players (remove if using confirmation flow)
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            photonView.RPC("NotifyFullRoom", RpcTarget.All);
        }
    }

    [PunRPC]
    private void NotifyFullRoom()
    {
        Debug.Log("Room is full!");
        // Add your full-room logic here
    }
}
