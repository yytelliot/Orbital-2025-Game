using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class ReloadStationController : MonoBehaviour, Interactable
{
    [SerializeField] private string stationID = "AmmoReload";
    [SerializeField] public GameObject miniGame;
    public static ReloadStationController Instance; // Singleton pattern
    public GameObject highlight;

    [Header("Events")]
    public GameEvent onAmmoMinigameComplete;

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



    private void OnEnable()
    {
        //highlight = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (other.tag == "Player")
        {
            highlight.SetActive(true);
        }

    }

    private void OnTriggerExit2D(UnityEngine.Collider2D other)
    {
        if (other.tag == "Player")
        {
            highlight.SetActive(false);
        }
    }

    public void Interact()
    {
        Debug.Log("Reload minigame start!");
        miniGame.SetActive(true);



    }

    public void SendResult(int score)
    { 
        Debug.Log(score + "end");
        onAmmoMinigameComplete.Raise(null, score);
    }
    

}
