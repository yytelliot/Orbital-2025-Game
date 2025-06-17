using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class CrossSceneNetworkManager : MonoBehaviourPunCallbacks
{
    
    [SerializeField] private GameEvent onAmmoConutChanged;
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

    
    public void SendTechnicianInteraction(Component sender, object data)
    {
        photonView.RPC(nameof(RPC_RelayToPilot), RpcTarget.All, sender, data);
    }

    [PunRPC]
    private void RPC_RelayToPilot(Component sender, object data)
    {
        if (SceneManager.GetActiveScene().name == "PilotScene")
        {
            int amount = (int)data;
            string caller = sender.ToString();
            Debug.Log($"Recieved {amount} from {caller}");
            // Forward to Pilot's UI
            onAmmoConutChanged.Raise(sender, data);
            //RoleSelectionManager.Instance?.ReceivePilotFeedback(stationID);
        }
    }
    

}
