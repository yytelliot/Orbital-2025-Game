using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteFactory : MonoBehaviour
{
    [SerializeField] private GameObject[] notePrefabs = new GameObject[4];
    [SerializeField] private Transform parent;

    public NoteFactory(GameObject[] prefabs, Transform noteParent)
    {
        notePrefabs = prefabs;
        parent = noteParent;
    }

    public GameObject CreateNote(int laneIndex, Vector3 position)
    {
        GameObject note = GameObject.Instantiate(notePrefabs[laneIndex], position, Quaternion.identity, parent);
        
        if (laneIndex == 0 || laneIndex == 1)
            note.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        else
            note.transform.rotation = Quaternion.Euler(0f, 0f, -90f);

        return note;
    }
}
