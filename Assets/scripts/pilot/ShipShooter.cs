using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShipShooter : MonoBehaviour
{


    [Header("Shoot Settings")]
    public GameObject ammoType; // bullet to be fired
    public Transform firePoint;     // point where bullet is fired from
    public float bulletSpeed = 1f; // bullet speed
    public float fireRate = 0.2f;  // time between shots

    [Header("Events")]
    public GameEvent onShotFired;
    Coroutine firingRoutine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetKeyDown("fire") && firingRoutine == null)
        {
            firingRoutine = StartCoroutine(AutoFire());
        }
            
    }

    // Auto Fire Coroutine
    IEnumerator AutoFire()
    {
        while (InputManager.GetKey("fire"))
        {
            ShootAtMouse();
            yield return new WaitForSeconds(fireRate);
        }
        firingRoutine = null;
        
    }

    // Shoots the bullet at the mouse
    void ShootAtMouse()
    {
        // get the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get the direction vector and angle from the ship's firePoint to the mouse
        Vector2 dir = (mousePosition - firePoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Spawn bullet
        GameObject bullet = Instantiate(ammoType,
                                        firePoint.position,
                                        Quaternion.identity);

        // Initialize bullet
        SimpleBulletBehavior bb = bullet.GetComponent<SimpleBulletBehavior>();
        bb.Initialize(dir, angle, bulletSpeed);

        // Raise onShoot Event
        onShotFired.Raise();
    }
}
