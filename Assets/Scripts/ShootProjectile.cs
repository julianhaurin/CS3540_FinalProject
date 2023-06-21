using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public AudioClip projectileSFX;
    public GameObject projectilePrefab;
    public float projectileSpeed = 100f;

    public float shieldCooldown = 5.0f;
    public GameObject ShieldPrefab;
    bool coolingDown = false;

    void Start() {
        
    }

    
    void Update() {
      if (!FindObjectOfType<LevelManager>().isPaused)
      {
        // shoots a projectile from the player when a fire1 button is pressed
        if (Input.GetButtonDown("Fire1")) {
          GameObject projectile = 
            Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation) as GameObject;

          Rigidbody rb = projectile.GetComponent<Rigidbody>();

          Vector3 cameraPitch = new Vector3(0f, (float)gameObject.transform.GetChild(0).gameObject.transform.forward.y, 0f);
          rb.AddForce((transform.forward + cameraPitch) * projectileSpeed, ForceMode.VelocityChange);

          AudioSource.PlayClipAtPoint(projectileSFX, transform.position);
        }

        /*if (Input.GetKeyDown(KeyCode.S) && Time.time > NextFire) {
          GameObject shield = Instantiate(ShieldPrefab);
          shield.transform.position = transform.position;
          shield.transform.position -= new Vector3(0, 2.5f, 0);
          NextFire = Time.time + shieldCooldown;
        }
        */

        if (Input.GetKeyDown(KeyCode.Tab) && !coolingDown)
        {
          GameObject shield = Instantiate(ShieldPrefab);
          shield.transform.position = transform.position;
          shield.transform.position -= new Vector3(0, 2.7f, 0);
          coolingDown = true;
        }
        else{
          shieldCooldown -= Time.deltaTime;
          if (shieldCooldown <= 0)
          {
            coolingDown = false;
            shieldCooldown += 5.0f;
          }
        }
      }
    }
}
