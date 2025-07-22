using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    [SerializeField] private SoundType[] footstepClips = new SoundType[]
        {
            SoundType.FOOTSTEP1,
            SoundType.FOOTSTEP2,
            SoundType.FOOTSTEP3,
            SoundType.FOOTSTEP4,
            SoundType.FOOTSTEP5
        }; 
    [SerializeField] private float footstepInterval = 0.4f;
    [SerializeField] private Animator animator;
    private float footstepTimer = 0f;
    private bool wasMoving = false;


    private void Update()
    {


        bool isCurrentlyMoving = animator.GetBool("isMoving");

        if (isCurrentlyMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                if (footstepClips.Length > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, footstepClips.Length);
                    SoundManagerTechnican.PlaySound(footstepClips[randomIndex]);
                }

                footstepTimer = footstepInterval;
            }
        }
        else if (wasMoving && !isCurrentlyMoving)
        {
            // Just stopped moving, reset timer
            footstepTimer = 0f;
        }

        wasMoving = isCurrentlyMoving;
    }
 
}
