using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundType
{
    //Player
    FOOTSTEP,               //0
    INTERACT,               //1

    //Fire
    FIRESTART,              //2
    FIREBURNING,            //3
    FIREEXTINGUISH,         //4

    //Reloading Station
    AMMO1,                  //5
    AMMO2,                  //6
    AMMO3,                  //7
    AMMO4,                  //8

    //Scanner Station
    TYPING,                 //9

    //General Minigame
    SUCCESS,                //10
    FAIL,                   //11

    //Settings
    SETTINGOPEN,            //12
    SETTINGCLICK            //13
}
public class SoundManagerTechnican : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private AudioSource audioSource;
    private static SoundManagerTechnican Instance; //singleton

    private void Awake()
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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        Instance.audioSource.PlayOneShot(Instance.soundList[(int)sound], volume);
    }
}
