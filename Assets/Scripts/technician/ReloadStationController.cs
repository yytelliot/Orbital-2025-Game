using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class ReloadStationController : MonoBehaviour, Interactable
{
    [SerializeField] private string stationID = "AmmoReload";
    public void Interact()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["PlayerRole"].ToString() == "Technician")
        {
            CrossSceneNetworkManager.Instance.SendTechnicianInteraction(stationID);

            // Local technician effects
            //PlayLocalEffects();
        }
    }
}
