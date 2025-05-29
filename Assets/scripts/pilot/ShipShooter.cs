using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShipShooter : MonoBehaviour
{


    [Header("Shoot Settings")]
    public GameObject bulletPrefab; // bullet to be fired
    public Transform firePoint;     // point where bullet is fired from
    // public float bulletSpeed = 10f; // bullet speed
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetKeyDown("fire"))
            ShootAtMouse();
    }

    void ShootAtMouse()
    {
        // get the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get the direction vector from the ship's firePoint to the mouse
        Vector2 dir = (mousePosition - firePoint.position).normalized;

        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab,
                                        firePoint.position,
                                        Quaternion.identity);

        // Move bullet in direction
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        float speed = bullet.GetComponent<BulletSimple>().speed;
        rb.velocity = dir * speed;

        // Rotate bullet to face direction of travel
        float rotationOffset = 90;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - rotationOffset;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
