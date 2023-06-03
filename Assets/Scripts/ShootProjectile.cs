using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public AudioClip projectileSFX;
    public GameObject projectilePrefab;
    public float projectileSpeed = 100f;

    void Start()
    {
        
    }

    
    void Update()
    {
        // shoots a projectile from the player when a fire1 button is pressed
        if (Input.GetButtonDown("Fire1")) {
          GameObject projectile = 
            Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation) as GameObject;

          Rigidbody rb = projectile.GetComponent<Rigidbody>();
          rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

          AudioSource.PlayClipAtPoint(projectileSFX, transform.position);
        }
    }
}
