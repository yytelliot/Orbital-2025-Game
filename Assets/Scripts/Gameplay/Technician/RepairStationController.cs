using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairStationController : MonoBehaviour, Interactable
{
    public GameEvent g;
    public void Interact()
    {
        g.Raise(this, null);
        Debug.Log("Repair minigame start!");

    }
}
