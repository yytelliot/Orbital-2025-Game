using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using System.Linq;

[RequireComponent(typeof(PhotonView))]
public class CrossSceneNetworkManager : MonoBehaviourPunCallbacks
{

    // [SerializeField] private GameEvent onShotFired;
    public static CrossSceneNetworkManager Instance { get; private set; }

    private List<GameEvent> gameEvents;
    private Dictionary<int, GameEvent> idToEvent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load all GameEvents from Resources/Events
            gameEvents = Resources.LoadAll<GameEvent>("Events").ToList();

            // Build look up dictionary to map eventIds
            idToEvent = gameEvents.Where(e => e != null).ToDictionary(e => e.eventId, e => e);
        }
        else
        {
            Destroy(gameObject);
        }
        

    }
    public void SendNetworkEvent(GameEvent gameEvent, Component sender, object data = null)
    {
        int id = gameEvent.eventId;

        photonView.RPC(nameof(
            RPC_RaiseEvent),
            RpcTarget.Others,
            id,
            sender.GetInstanceID(),
            data);
    }

    [PunRPC]
    private void RPC_RaiseEvent(int eventId, int senderId, object data)
    {
        GameEvent gameEvent = idToEvent[eventId];
        Component sender = FindInstanceById(senderId);
        gameEvent.Raise(sender, data);
    }

    private Component FindInstanceById(int id)
    {
        foreach (Component component in FindObjectsOfType<MonoBehaviour>())
        {
            if (id == component.GetInstanceID())
            {
                return component;
            }
        }
        return null;
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


    // public void SendTechnicianInteraction(string stationID)
    // {
    //     photonView.RPC(nameof(RPC_RelayToPilot), RpcTarget.All, stationID);
    // }

    // [PunRPC]
    // private void RPC_RelayToPilot(string stationID)
    // {
    //     if (SceneManager.GetActiveScene().name == "PilotScene")
    //     {
    //         // Forward to Pilot's UI
    //         onShotFired.Raise(this, null);
    //         //RoleSelectionManager.Instance?.ReceivePilotFeedback(stationID);
    //     }
    // } 


}
