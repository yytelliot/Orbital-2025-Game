using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShipShooter : MonoBehaviour
{


    [Header("Shoot Settings")]
    public GameObject bulletPrefab; // bullet to be fired
    public Transform firePoint;     // point where bullet is fired from
    public float bulletSpeed = 1f; // bullet speed
    

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

        // get the direction vector and angle from the ship's firePoint to the mouse
        Vector2 dir = (mousePosition - firePoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab,
                                        firePoint.position,
                                        Quaternion.identity);

        // Initialize bullet
        SimpleBulletBehavior bb = bullet.GetComponent<SimpleBulletBehavior>();
        bb.Initialize(dir, angle, bulletSpeed);
    }
}
