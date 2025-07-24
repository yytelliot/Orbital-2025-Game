using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStationController : MonoBehaviour, Interactable
{

    [Header("References")]
    [SerializeField] private GameObject difficultyUI;
    [SerializeField] private PlayerController player;

    [Header(("Events"))]
    [SerializeField] private GameEvent ScanComplete;
    public static CraftingStationController Instance; // Singleton pattern
    public GameObject highlight;

   
    //public GameEvent onCraftingdifficultyUIComplete;
    //public GameEvent onCraftingdifficultyUIStart;

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
        Debug.Log("Crafting difficultyUI start!");
        difficultyUI.SetActive(true);

        //Sound Effect .....
        SoundManagerTechnican.PlaySound(SoundType.SCANNEROPEN, 1.5f);

    }
    
    public void SendResult(bool result, int level)
    { 
        player.SetCanMove(true);

        if (result)
        {

            ScanComplete.RaiseNetworked(this, level);
            //Sound Effect .....
            SoundManagerTechnican.PlaySound(SoundType.SUCCESS);
        }
        else
        {
            //Sound Effect .....
            SoundManagerTechnican.PlaySound(SoundType.FAIL);
        }
        Debug.Log("You " + (result ? "Pass" : "Fail"));

    }

    
}
