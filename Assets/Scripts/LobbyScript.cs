using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviourPunCallbacks
{
    public Button pilotButton;
    public Button technicianButton;
    public Button confirmButton;
    private string selectedRole = "";
    private static Dictionary<int, string> playerRoles = new();

    public void Start()
    {
        confirmButton.interactable = false;
        pilotButton.onClick.AddListener(() => SelectRole("Pilot"));
        technicianButton.onClick.AddListener(() => SelectRole("Technician"));
        confirmButton.onClick.AddListener(SetReady);
    }

    public void Update()
    {
        
    }

    void SelectRole(string role)
    {
        selectedRole = role;
    }

    void SetReady()
    {
        photonView.RPC("ConfirmRole", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, selectedRole);
    }

    [PunRPC]
    void ConfirmRole(int actorNumber, string role)
    {
        playerRoles[actorNumber] = role;

        if (playerRoles.Count == 2 && playerRoles.Values.Contains("Pilot") && playerRoles.Values.Contains("Technician"))
        {
            confirmButton.interactable = true;
            foreach (var entry in playerRoles)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == entry.Key)
                {
                    if (entry.Value == "Pilot") PhotonNetwork.LoadLevel("PilotScene");
                    else if (entry.Value == "Technician") PhotonNetwork.LoadLevel("TechnicianScene");
                }
            }
        }
    }
    

    public void ChooseRole(string role)
    {
        selectedRole = role;
    }

    public void ConfirmRole()
    {
        ExitGames.Client.Photon.Hashtable props = new();
        props["role"] = selectedRole;
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("PilotScene");
        }
    }
    
}
