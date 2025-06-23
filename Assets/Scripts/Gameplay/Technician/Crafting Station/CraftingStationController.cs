using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStationController : MonoBehaviour, Interactable
{
    [SerializeField] public GameObject miniGame;
    [SerializeField] public PlayerController player;
    public static CraftingStationController Instance; // Singleton pattern
    public GameObject highlight;

    [Header("Events")]
    //public GameEvent onCraftingMinigameComplete;
    public GameEvent onCraftingMinigameStart;

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
        player.SetCanMove(false);
        Debug.Log("Crafting minigame start!");
        miniGame.SetActive(true);
        onCraftingMinigameStart.Raise(this, null);

    }

    
}
