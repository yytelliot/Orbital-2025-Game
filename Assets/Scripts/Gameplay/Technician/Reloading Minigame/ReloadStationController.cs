using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;



public class ReloadStationController : MonoBehaviour, IInteractable
{
    
    [SerializeField] private GameObject miniGame;
    [SerializeField] private PlayerController player;
    public static ReloadStationController Instance; // Singleton pattern
    [SerializeField] private GameObject highlight;

    [Header("Events")]
    [SerializeField] private GameEvent onAmmoMinigameComplete;
    [SerializeField] private GameEvent onAmmoMinigameStart;
    [SerializeField] private ProgressBar progressBar;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            highlight.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            highlight.SetActive(false);
        }
    }

    public void Interact()
    {
        player.SetCanMove(false);
        Debug.Log("Reload minigame start!");
        miniGame.SetActive(true);
        onAmmoMinigameStart.Raise(this, null);

        //Sound Effect .....
        SoundManagerTechnican.PlaySound(SoundType.RELOADOPEN);

    }

    public void SendResult(int score)
    { 
        player.SetCanMove(true);
        Debug.Log(score + "end");
        onAmmoMinigameComplete.RaiseNetworked(this, score);
    }
    

}
