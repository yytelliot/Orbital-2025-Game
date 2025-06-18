using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] public GameObject miniGame;
    
    [Header("Note Prefabs (4 lanes)")]
    public GameObject[] notePrefabs = new GameObject[4]; // Assign 4 prefabs in Inspector

    [Header("Timing and Spawning")]
    [SerializeField] private float spawnInterval = 0.15f; // Spawn every 0.5 seconds
    [SerializeField] private float gameDuration = 15f;   // Duration of the tapping phase
    public Transform[] ammoPos = new Transform[4];        // Where notes start spawning (e.g. top of the screen)

    [SerializeField] private float verticalPos;
    [SerializeField] private Transform noteParent;
    


    //public int numberOfNotes = 10;              // How many notes to spawn
    //public float spacingBetweenNotes = 2f;      // Vertical spacing between notes


    //[Header("Note Keys")]
    //public KeyCode[] keyOptions = new KeyCode[4]; 

    private float startTime;
    private bool isRunning = false;

    public void StartMinigame()
    {
        //SpawnNotePattern();
        StartCoroutine(GameLoop());
        
        
    }

    public void ResetMinigame()
    {
        StopAllCoroutines(); // Stop previous coroutine if needed

        // Destroy leftover notes
        foreach (Transform child in noteParent)
        {
            Destroy(child.gameObject);
        }

        AmmoScroller.Instance?.SetScrolling(true); // Ready to scroll again
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        startTime = Time.time;
        isRunning = true;

        while (Time.time - startTime < gameDuration)
        {
            while (AmmoScroller.Instance == null || !AmmoScroller.Instance.IsScrolling())
            {
                yield return null;
            }

            SpawnNote();
            yield return new WaitForSeconds(spawnInterval);
        }

        //Game ends here
        int score = AmmoScroller.Instance.GetScore();
        ReloadStationController.Instance.SendResult(score);
        isRunning = false;
        AmmoScroller.Instance?.SetScrolling(false);
        Debug.Log("Times up");
        miniGame.SetActive(false);
        
        foreach (Transform child in noteParent)
        {
            Destroy(child.gameObject);
        }


        
        
 
    }

    void SpawnNote()
    {

        // Pick a random lane (0–3)
        int laneIndex = Random.Range(0, 4);

        // Calculate position (x for lane, y for spacing)
        Vector3 spawnPos = new Vector3(ammoPos[laneIndex].position.x, verticalPos, 0f);
        
        GameObject note = Instantiate(notePrefabs[laneIndex], spawnPos, Quaternion.identity, noteParent);

        // Fix any inherited distortion
        //note.transform.localScale = Vector3.one;

        // Instantiate the note prefab for that lane
        if (laneIndex == 0 || laneIndex == 1)
        {
            note.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else
        {
            note.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }

        // Assign the key to the note script
        /*AmmoNoteObject noteScript = note.GetComponent<AmmoNoteObject>();
        if (noteScript != null && keyOptions.Length > laneIndex)
        {
            noteScript.keyToPress = keyOptions[laneIndex];
        }*/
    }

    
    /*void SpawnNotePattern()
    {
        for (int i = 0; i < numberOfNotes; i++)
        {
            // Pick a random lane (0–3)
            int laneIndex = Random.Range(0, 4);

            // Calculate position (x for lane, y for spacing)
            Vector3 spawnPos = new Vector3(ammoPos[laneIndex].position.x, i * spacingBetweenNotes, 0f);
            
            GameObject note = Instantiate(notePrefabs[laneIndex], spawnPos, Quaternion.identity, noteParent);

            // Fix any inherited distortion
            //note.transform.localScale = Vector3.one;

            // Instantiate the note prefab for that lane
            if (laneIndex == 0 || laneIndex == 1)
            {
                note.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else
            {
                note.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }


        }
    } */
}
