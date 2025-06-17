using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AmmoScroller : MonoBehaviour
{
    public float beatTempo;
    [SerializeField] private bool ToggleScrolling = false;
    [SerializeField] private TMP_Text statusText;
    private int ammoReloaded = 0;
    public static AmmoScroller Instance; // Singleton pattern

    void Awake()
    {
        Instance = this;
        beatTempo /= 60f; // units at per seconds
    }

    // Update is called once per frame
    void Update()
    {
        if (!ToggleScrolling)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleScrolling = true;
            }
        }
        else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);

        }
    }

    public void SetScrolling(bool shouldScroll)
    {
        ToggleScrolling = shouldScroll;
        //Debug.Log(shouldScroll ? "Scrolling RESUMED" : "Scrolling PAUSED");
    }

    public bool IsScrolling()
    {
        return ToggleScrolling;
    }

    public void AddScore()
    {
        ammoReloaded++;
        statusText.text = "Ammo: " + ammoReloaded;
    }

    public int GetScore()
    {
        return ammoReloaded;
    }
}
