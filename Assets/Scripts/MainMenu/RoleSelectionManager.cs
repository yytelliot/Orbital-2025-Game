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
    [SerializeField] private Button backButton;
    [SerializeField] private TMP_Text statusText;


    private string selectedRole = "";
    private bool hasConfirmed = false;
    private PhotonView myPhotonView;
    private Dictionary<int, string> playerRoles = new Dictionary<int, string>();
    public static RoleSelectionManager Instance; 

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        myPhotonView = GetComponent<PhotonView>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (myPhotonView == null) return;

        // Initialize buttons
        confirmButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        technicianButton.onClick.AddListener(() => SelectRole("Technician"));
        pilotButton.onClick.AddListener(() => SelectRole("Pilot"));
        confirmButton.onClick.AddListener(ConfirmSelection);
        backButton.onClick.AddListener(CancelSelection);

        UpdateStatus("Select your role");
    }

    private void SelectRole(string role)
    {
        if (hasConfirmed) return;

        selectedRole = role;
        confirmButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);

        // Disable both buttons for local player
        technicianButton.interactable = false;
        pilotButton.interactable = false;

        var props = new ExitGames.Client.Photon.Hashtable
        {
            { "PlayerRole", this.selectedRole }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        // Store the selection
        playerRoles[PhotonNetwork.LocalPlayer.ActorNumber] = selectedRole;

        // Sync the selection to other players
        myPhotonView.RPC(nameof(SyncRoleSelection), RpcTarget.All, selectedRole, PhotonNetwork.LocalPlayer.ActorNumber);

        UpdateStatus($"You selected: {selectedRole}\nPress Confirm to lock in your choice");

    }

    [PunRPC]
    private void SyncRoleSelection(string selectedRole, int actorNumber)
    {
        // Store the remote player's selection
        playerRoles[actorNumber] = selectedRole;

        // If this isn't our own selection
        if (actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            // Disable both buttons if any player has selected
            if (playerRoles.Count > 0)
            {
                technicianButton.interactable = false;
                pilotButton.interactable = false;

                confirmButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(true);

            }

            // If we haven't selected yet, auto-assign the other role
            if (string.IsNullOrEmpty(this.selectedRole))
            {
                this.selectedRole = (selectedRole == "Pilot") ? "Technician" : "Pilot";
                playerRoles[PhotonNetwork.LocalPlayer.ActorNumber] = this.selectedRole;
                
                var props = new ExitGames.Client.Photon.Hashtable
                {
                    { "PlayerRole", this.selectedRole }
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                UpdateStatus($"Other player selected {selectedRole}\nYou were assigned: {this.selectedRole}");
            }
            else
            {
                UpdateStatus($"Other player selected {selectedRole}\nYou selected: {this.selectedRole}");
            }
        }
    }

    private void ConfirmSelection()
    {
        if (string.IsNullOrEmpty(selectedRole)) return;

        hasConfirmed = true;

        var props = new ExitGames.Client.Photon.Hashtable
        {
            { "HasConfirmed", true }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        confirmButton.interactable = false;
        backButton.interactable = true;

        UpdateStatus("Waiting for other player to confirm...");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("confirm run");
            //Invoke(nameof(CheckAllPlayersReady), 1f); //delay for RPC
            
        }
        
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

    public void CancelSelection()
    {
        selectedRole = "";
        hasConfirmed = false;
        playerRoles.Remove(PhotonNetwork.LocalPlayer.ActorNumber);

        confirmButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);

        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "PlayerRole", null },
            { "HasConfirmed", false }
        });

        myPhotonView.RPC(nameof(ResetRoles), RpcTarget.All);
    }

    [PunRPC]
    private void LoadRoleScene()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out object role)) return;
        Debug.Log(role);
        PhotonNetwork.LoadLevel((string)role + "Scene");
    }

    [PunRPC]
    private void ResetRoles()
    {
        selectedRole = "";
        hasConfirmed = false;
        playerRoles.Clear();

        technicianButton.interactable = true;
        pilotButton.interactable = true;
        confirmButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        confirmButton.interactable = true;
        backButton.interactable = true;

        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "PlayerRole", null },
            { "HasConfirmed", false }
        });

        UpdateStatus("Select your role");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Sync existing selections to the new player
        foreach (var kvp in playerRoles)
        {
            myPhotonView.RPC(nameof(SyncRoleSelection), newPlayer, kvp.Value, kvp.Key);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("properties run");
            CheckAllPlayersReady();
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null) statusText.text = message;
    }

    void OnDestroy()
    {
        if (PhotonNetwork.IsConnected && myPhotonView != null && myPhotonView.IsMine)
        {
            PhotonNetwork.RemoveRPCs(myPhotonView);
        }
    }

    
}
