using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{

    [Tooltip("Unique ID - set once only")]
    public int eventId = -1;

    // list of active event listeners
    public List<GameEventListener> listeners = new List<GameEventListener>();

    // calling Raise() signals that an the GameEvent is currently happening,
    // invoking all event listeners
    public void Raise(Component sender, object data = null)
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised(sender, data);
        }
    }

    public void Raise()
    {
        Raise(null, null);
    }

    // Add/remove a new event listener
    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }

}
