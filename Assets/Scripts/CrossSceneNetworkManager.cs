using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class CrossSceneNetworkManager : MonoBehaviourPunCallbacks
{
    
    [SerializeField] private GameEvent onShotFired;
    public static CrossSceneNetworkManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*public void SendTechnicianInteraction()
    {
        photonView.RPC(nameof(RPC_RelayToPilot), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_RelayToPilot()
    {
        if (SceneManager.GetActiveScene().name == "PilotScene")
        {
            // Forward to Pilot's UI
            onShotFired.Raise();
            
            //RoleSelectionManager.Instance?.ReceivePilotFeedback();
        }
    }*/

    
    public void SendTechnicianInteraction(string stationID)
    {
        photonView.RPC(nameof(RPC_RelayToPilot), RpcTarget.All, stationID);
    }

    [PunRPC]
    private void RPC_RelayToPilot(string stationID)
    {
        if (SceneManager.GetActiveScene().name == "PilotScene")
        {
            // Forward to Pilot's UI
            onShotFired.Raise(this, null);
            //RoleSelectionManager.Instance?.ReceivePilotFeedback(stationID);
        }
    }
    

}
