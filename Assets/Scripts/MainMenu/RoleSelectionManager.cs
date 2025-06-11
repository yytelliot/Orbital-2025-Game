using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using WebSocketSharp;

[RequireComponent(typeof(PhotonView))]
public class RoleSelectionManager : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    [SerializeField] private Button technicianButton;
    [SerializeField] private Button pilotButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TMP_Text statusText;

    private string selectedRole = "";
    private bool hasConfirmed = false;
    private string OtherPlayerSelected = "";
    private PhotonView myPhotonView;

    private void Awake()
    {
        if (FindObjectsOfType<RoleSelectionManager>().Length > 1) //singleton class, only one exist
        {
            Destroy(gameObject);
            return;
        }

        // Get or add PhotonView component with new name
        myPhotonView = GetComponent<PhotonView>();
        if (myPhotonView == null)
        {
            myPhotonView = gameObject.AddComponent<PhotonView>();
            myPhotonView.OwnershipTransfer = OwnershipOption.Takeover;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (myPhotonView == null) return;   //ensures there is always a main host

        // Initialize buttons
        confirmButton.gameObject.SetActive(false); // Hide on startup
        technicianButton.onClick.AddListener(() => SelectRole("Technician"));
        pilotButton.onClick.AddListener(() => SelectRole("Pilot"));
        confirmButton.onClick.AddListener(ConfirmSelection);

        UpdateStatus("Select your role");
    }

    private void SelectRole(string role)
    {
        if (hasConfirmed) return;

        selectedRole = role;
        confirmButton.gameObject.SetActive(true);

        // Visual feedback
        if (OtherPlayerSelected.IsNullOrEmpty())
        {
            technicianButton.interactable = role != "Technician";
            pilotButton.interactable = role != "Pilot";
        }
        else
        {
            technicianButton.interactable = false;
            pilotButton.interactable = false;
        }
        

        UpdateStatus($"Selected: {role}\nPress Confirm");
    }

    /*private void SelectTechnicianRole()
    {
        if (hasConfirmed) return;

        selectedRole = "Technician";
        confirmButton.gameObject.SetActive(true);

        // Visual feedback
        technicianButton.interactable = false;

        UpdateStatus($"Selected: {selectedRole}\nPress Confirm");
    }

    private void SelectPilotRole()
    {
        if (hasConfirmed) return;

        selectedRole = "Pilot";
        confirmButton.gameObject.SetActive(true);

        // Visual feedback
        pilotButton.interactable = false;

        UpdateStatus($"Selected: {selectedRole}\nPress Confirm");
    }*/
    


    private void ConfirmSelection()
    {
        if (string.IsNullOrEmpty(selectedRole)) return;

        hasConfirmed = true;

        // Store selection in Photon custom properties
        var props = new ExitGames.Client.Photon.Hashtable
        {
            {"PlayerRole", selectedRole},
            {"HasConfirmed", true}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        RemoveThisRoleOption(selectedRole);

        // Lock all buttons
        technicianButton.interactable = false;
        pilotButton.interactable = false;
        confirmButton.interactable = false;

        UpdateStatus($"Waiting for other player...\nYou are: {selectedRole}");



        CheckAllPlayersReady();
    }

    private void CheckAllPlayersReady()
    {
        if (myPhotonView == null || PhotonNetwork.CurrentRoom.PlayerCount < 2) return;

        bool allConfirmed = true;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("HasConfirmed") || !(bool)player.CustomProperties["HasConfirmed"])
            {
                allConfirmed = false;
                break;
            }
        }

        if (allConfirmed && PhotonNetwork.IsMasterClient) 
        {
            myPhotonView.RPC(nameof(LoadRoleScene), RpcTarget.All); 
        }
    }

    public void RemoveThisRoleOption(string selectedRole)
    {
        myPhotonView.RPC(nameof(RemoveRoleOption), RpcTarget.All, selectedRole);
    }

    [PunRPC]
    private void LoadRoleScene()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out object role)) return;

        PhotonNetwork.LoadLevel((string)role + "Scene");
    }

    [PunRPC]
    private void RemoveRoleOption(string selectedRole)
    {
        OtherPlayerSelected = selectedRole;
        // Visual feedback
        technicianButton.interactable = selectedRole != "Technician";
        pilotButton.interactable = selectedRole != "Pilot";
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // If any player presses on the confirm button and update their player properties, this will run and check if all players are ready.
        if (myPhotonView != null && PhotonNetwork.IsMasterClient && targetPlayer != PhotonNetwork.LocalPlayer)
        {
            CheckAllPlayersReady();
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null) statusText.text = message;
    }
    void OnDestroy()
    {
        if (PhotonNetwork.IsConnected && myPhotonView != null)
        {
            PhotonNetwork.RemoveRPCs(myPhotonView);
        }
    }

}
