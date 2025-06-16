using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkedEvents
{
    public static void Broadcast(GameEvent e, Component sender, object data = null)
    {
        // local
        e.Raise(sender, data);

        // network
        CrossSceneNetworkManager.Instance.SendNetworkEvent(e, sender, data);
    }
}
