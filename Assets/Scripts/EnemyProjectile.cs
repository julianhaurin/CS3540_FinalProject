using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 2f;
    public int damageAmount = 20;
    
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed);
    }

    void OnTriggerEnter(Collider other) {
      if (!FindObjectOfType<LevelManager>().isPaused)
      {
        if(other.gameObject.tag == "Shield") 
        {
          //shield destroys projectile
          Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player") {
          
          if (Input.GetKey(KeyCode.LeftShift)) {
            // Debug.Log("projectile blocked by player");
            Destroy(gameObject);
            
          } else {
            // Debug.Log("player hit");
            var playerHealth = other.GetComponent<Health>();
            playerHealth.TakeDamage(damageAmount);  
          }
          
        }
      }
    }
}
