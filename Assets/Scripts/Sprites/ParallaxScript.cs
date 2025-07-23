using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    private float startPosX, startPosY, length, height;
    public GameObject cam;
    public float parallaxEffect; // speed that bg should move relative to camera

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }


    void LateUpdate()
    {
        // calculate bg distance movement
        float distanceX = cam.transform.position.x * parallaxEffect; // 0 = move with cam || 1 won't move
        float movementX = cam.transform.position.x * (1 - parallaxEffect);

        float distanceY = cam.transform.position.y * parallaxEffect;
        float movementY = cam.transform.position.y * (1 - parallaxEffect);

        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);

        if (movementX > startPosX + length)
        {
            startPosX += length;
        }
        else if (movementX < startPosX - length)
        {
            startPosX -= length;
        }
        else if (movementY > startPosY + height)
        {
            startPosY += height;
        }
        else if (movementY < startPosY - length)
        {
            startPosY -= length;
        }
    }
}
