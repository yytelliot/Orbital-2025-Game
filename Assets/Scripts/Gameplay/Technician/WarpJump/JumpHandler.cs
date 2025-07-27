using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ButtonUp;
    [SerializeField] private GameObject ButtonDown;

    [Header("Events")]
    [SerializeField] private GameEvent JumpComplete;
    [SerializeField] private GameEvent EnergyProgressBarUpdate;

    private bool jumpReady = false;
    private Color originalUpColor;
    private Color originalDownColor;

    private void Awake()
    {
        if (ButtonUp != null)
            originalUpColor = ButtonUp.GetComponent<SpriteRenderer>().color;
        if (ButtonDown != null)
            originalDownColor = ButtonDown.GetComponent<SpriteRenderer>().color;

        ResetState();
    }

    private void Update()
    {

    }

    public void OnJumpReady(Component sender, object data)
    {
        SetButtonGreyedOut(ButtonDown, false);
        SetButtonGreyedOut(ButtonUp, false);
        jumpReady = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            ButtonDown.SetActive(true);
            ButtonUp.SetActive(false);

            if (jumpReady)
            {
                JumpComplete.RaiseNetworked(this, null);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ButtonDown.SetActive(false);
            ButtonUp.SetActive(true);

            if (jumpReady)
            {
                Debug.Log("resetstate");
                //PlayerStats.Instance.AddGalaxiesJumped(1);
                EnergyProgressBarUpdate.Raise(this, 0f);
                ResetState();
            }
        }
    }

    private Color DesaturateColor(Color original)
    {
        return Color.gray;   
    }

    private void SetButtonGreyedOut(GameObject button, bool greyOut)
    {
        SpriteRenderer sr = button.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        if (greyOut)
        {
            // Desaturate based on the original color
            sr.color = DesaturateColor(sr.color);
        }
        else
        {
            // Restore original color
            sr.color = Color.white;
        }
    }

    private void ResetState()
    {
        
        ButtonDown.SetActive(false);
        ButtonUp.SetActive(true);
        

        SetButtonGreyedOut(ButtonDown, true);
        SetButtonGreyedOut(ButtonUp, true);
        jumpReady = false;
    }
}
