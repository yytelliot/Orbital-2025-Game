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

    // WARNING: unfortunately due to limitations with what you can send over via RPC, the sender
    // of networked events will be filled as the component's PhotonView (or null if there isnt one)
    public void SendNetworkEvent(GameEvent gameEvent, Component sender, object data = null)
    {
        int evtId = gameEvent.eventId;
        PhotonView pv = sender.GetComponent<PhotonView>();

        // if theres a photon view id send it, else default to 0 (null)
        int senderViewId = pv != null
            ? pv.ViewID
            : 0;

        photonView.RPC(nameof(
            RPC_RaiseEvent),
            RpcTarget.Others,
            evtId,
            senderViewId,
            data);
    }

    [PunRPC]
    private void RPC_RaiseEvent(int eventId, int senderViewId, object data)
    {
        GameEvent gameEvent = idToEvent[eventId];

        // use PhotonView.Find to get the photonview component across the network
        PhotonView senderPv = PhotonView.Find(senderViewId);

        // send the photonview
        Component sender = senderPv;
            
        gameEvent.Raise(sender, data);
    }


    // original function to find instance by component ID, no need for it now
    /*
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
    */


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
