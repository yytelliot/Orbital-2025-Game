using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairStationController : MonoBehaviour, Interactable
{
    public GameEvent g;
    public void Interact()
    {
        g.Raise(this, 0.02f);
        Debug.Log("Repair minigame start!");

    }
}
