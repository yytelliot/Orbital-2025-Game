using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class DifficultyUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject easyArrow;
    [SerializeField] private GameObject mediumArrow;
    [SerializeField] private GameObject hardArrow;
    [SerializeField] private GameObject miniGame;

    [Header("Event")]
    public GameEvent onCraftingdifficultyUIStart;


    private Difficulty selected = Difficulty.None;
    private bool awaitingConfirmation = false;

    

    void OnEnable()
    {
        ResetSelection();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleInput(Difficulty.Easy);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleInput(Difficulty.Medium);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleInput(Difficulty.Hard);
        }
    }

    void HandleInput(Difficulty input)
    {
        if (!awaitingConfirmation)
        {
            selected = input;
            awaitingConfirmation = true;
            ShowOnlySelectedArrow(input);
        }
        else
        {
            if (selected == input)
            {
                ConfirmSelection(input);
            }
            else
            {
                // Player changed mind
                ResetSelection();
            }
        }
    }

    void ConfirmSelection(Difficulty difficulty)
    {
        miniGame.SetActive(true);
        
        switch (difficulty)
        {
            case Difficulty.Easy:
                onCraftingdifficultyUIStart.Raise(this, Difficulty.Easy);
                break;
            case Difficulty.Medium:
                onCraftingdifficultyUIStart.Raise(this, Difficulty.Medium);
                break;
            case Difficulty.Hard:
                onCraftingdifficultyUIStart.Raise(this, Difficulty.Hard);
                break;
        }
        gameObject.SetActive(false); // Hide difficulty UI
    }

    void ResetSelection()
    {
        selected = Difficulty.None;
        awaitingConfirmation = false;

        // Show all arrows
        easyArrow.SetActive(true);
        mediumArrow.SetActive(true);
        hardArrow.SetActive(true);
    }

    void ShowOnlySelectedArrow(Difficulty selected)
    {
        easyArrow.SetActive(selected == Difficulty.Easy);
        mediumArrow.SetActive(selected == Difficulty.Medium);
        hardArrow.SetActive(selected == Difficulty.Hard);
    }
}
