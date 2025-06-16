using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventExtensions
{
    public static void RaiseNetworked(
        this GameEvent e,
        Component sender,
        object data = null
    )
    {
        NetworkedEvents.Broadcast(e, sender, data);
    }

    public static void RaiseNetworked(this GameEvent e, object data = null)
    {
        NetworkedEvents.Broadcast(e, null, data);
    }
}
