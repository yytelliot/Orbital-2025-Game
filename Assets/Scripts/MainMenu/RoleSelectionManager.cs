using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

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

        confirmButton.gameObject.SetActive(false);
        UpdateStatus("Select your role");
    }

    private void SelectRole(string role)
    {
        if (hasConfirmed || myPhotonView == null) return;

        selectedRole = role;
        confirmButton.gameObject.SetActive(true);

        // Visual feedback
        technicianButton.interactable = (role != "Technician");
        pilotButton.interactable = (role != "Pilot");

        UpdateStatus($"Selected: {role}\nPress Confirm");
    }

    private void ConfirmSelection()
    {
        if (string.IsNullOrEmpty(selectedRole) || myPhotonView == null) return;

        hasConfirmed = true;

        // Store selection in Photon custom properties
        var props = new ExitGames.Client.Photon.Hashtable
        {
            {"PlayerRole", selectedRole},
            {"HasConfirmed", true}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        // Lock all buttons
        technicianButton.interactable = false;
        pilotButton.interactable = false;
        confirmButton.interactable = false;

        UpdateStatus($"Waiting for other player...\nYou are: {selectedRole}");

        // Check if both players are ready
        /*if (PhotonNetwork.IsMasterClient)
        {
            CheckAllPlayersReady();
        }*/
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

        if (allConfirmed && PhotonNetwork.IsMasterClient) //& AllPlayersReady())
        {
            myPhotonView.RPC(nameof(LoadRoleScene), RpcTarget.All); //photonView.RPC("LoadRoleScene", RpcTarget.All);
        }
    }

    [PunRPC]
    private void LoadRoleScene()
    {
        //string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["PlayerRole"];

        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out object role)) return;

        PhotonNetwork.LoadLevel((string)role + "Scene");

        /*if (role == "Pilot")
        {
            PhotonNetwork.LoadLevel("PilotScene");
        }
        else if (role == "Technician")
        {
            PhotonNetwork.LoadLevel("TechnicianScene");
        }*/
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // If master client and this is another player updating their status
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

    /*private bool AllPlayersReady()
    {
        if (!PhotonNetwork.InRoom) return false;
        
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || 
                !(bool)player.CustomProperties["IsReady"])
            {
                return false;
            }
        }
        return true;
    }*/

}
