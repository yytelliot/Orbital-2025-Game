using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public UnityEvent response;

    // When this event listener is enabled/disabled, register/deregister it in the list of events
    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    // Invoke the event when it is raised
    public void OnEventRaised()
    {
        response.Invoke();
    }
}
