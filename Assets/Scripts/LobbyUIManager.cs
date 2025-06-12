using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyUIManager : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    [SerializeField] private Button pilotButton;
    [SerializeField] private Button technicianButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TMP_Text statusText;

    private string selectedRole = "";
    private bool hasConfirmed = false;


    private void Start()
    {
        pilotButton.onClick.AddListener(() => SelectRole("Pilot"));
        technicianButton.onClick.AddListener(() => SelectRole("Technician"));
        confirmButton.onClick.AddListener(ConfirmSelection);
        
        UpdateUI();
        
    }

    private void SelectRole(string role)
    {
        if (hasConfirmed) return;
        
        selectedRole = role;
        statusText.text = $"Selected: {role}";
        
        // Visual feedback
        pilotButton.interactable = (role != "Pilot");
        technicianButton.interactable = (role != "Technician");
    }

    private void ConfirmSelection()
    {
        if (string.IsNullOrEmpty(selectedRole)) return;
        
        hasConfirmed = true;
        
        // Store selection in Photon custom properties
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            {"PlayerRole", selectedRole},
            {"IsReady", true}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        
        // Lock all buttons
        pilotButton.interactable = false;
        technicianButton.interactable = false;
        confirmButton.interactable = false;
        
        statusText.text = $"Waiting for other player...\n(You're {selectedRole})";
        
        // If master client and both ready, start game
        if (PhotonNetwork.IsMasterClient)
        {
            CheckAllPlayersReady();
        }
    }

    private void CheckAllPlayersReady()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2) return;
        
        bool allReady = true;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
            {
                allReady = false;
                break;
            }
        }

        if (allReady)
        {
            PhotonNetwork.LoadLevel("PilotScene");
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // If master client and this is another player updating their ready status
        if (PhotonNetwork.IsMasterClient && targetPlayer != PhotonNetwork.LocalPlayer)
        {
            CheckAllPlayersReady();
        }
    }

    private void UpdateUI()
    {
        confirmButton.interactable = !string.IsNullOrEmpty(selectedRole) && !hasConfirmed;
    }

}
