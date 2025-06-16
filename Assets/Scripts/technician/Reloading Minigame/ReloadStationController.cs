using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class ReloadStationController : MonoBehaviour, Interactable
{
    [SerializeField] private string stationID = "AmmoReload";
    [SerializeField] public GameObject miniGame;
    public GameObject highlight;

    private void OnEnable()
    {
        //highlight = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (other.tag == "Player")
        {
            highlight.SetActive(true);
        }
        
    }

    private void OnTriggerExit2D(UnityEngine.Collider2D other)
    {
        if (other.tag == "Player")
        {
            highlight.SetActive(false);
        }
    }

    public void Interact()
    {
        Debug.Log("Reload minigame start!");

        if (PhotonNetwork.LocalPlayer.CustomProperties["PlayerRole"].ToString() == "Technician")
        {
            //miniGame.SetActive(true);
            CrossSceneNetworkManager.Instance.SendTechnicianInteraction(stationID);
            Debug.Log("hit");
            // Local technician effects
            //PlayLocalEffects();
        }
    }
}
