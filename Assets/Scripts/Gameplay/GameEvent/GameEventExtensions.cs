using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class GameEventExtensions
{
    public static void RaiseNetworked(
        this GameEvent e,
        Component sender,
        object data
    )
    {
        NetworkedEvents.Broadcast(e, sender, data);
    }

    public static void RaiseNetworked(this GameEvent e, object data)
    {
        NetworkedEvents.Broadcast(e, null, data);
    }

    public static void RaiseNetworked(this GameEvent e, Component sender)
    {
        NetworkedEvents.Broadcast(e, sender, null);
    }

    public static void RaiseNetworked(this GameEvent e)
    {
        NetworkedEvents.Broadcast(e, null, null);
    }
    
}
