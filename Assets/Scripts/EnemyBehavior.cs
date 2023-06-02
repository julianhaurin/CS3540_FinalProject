using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject enemyAttack;
    [Range(1, 20)]
    [Tooltip("How likely the enemy is to attack during each frame")]
    public int attackRate = 1;

    [Range(1, 20)]
    [Tooltip("How likely the enemy is to dash during each frame")]
    public int dashRate = 1;

    public float enemySpeed = 100;
    public AudioClip dashSFX;
    
    private bool clockwise = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (clockwise)
        {
            transform.Rotate(Vector3.up, 120 * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.up, -120 * Time.deltaTime);
        }

        if (Random.Range(0, 50) == 0)
        {
            clockwise = !clockwise;
        }

        if (Random.Range(0, 500) < attackRate)
        {
            Attack();
        } 
        
        if (Random.Range(0, 500) < dashRate)
        {
            Dash();
        } 

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.tag == "EnemyProjectile"))
        {
            Destroy(gameObject);
        }
    }

    private void Attack()
    {
        Instantiate(enemyAttack, transform.position + transform.forward + transform.up, transform.rotation);
    }

    private void Dash()
    {
        float step = enemySpeed * Time.deltaTime;

        transform.position += transform.forward * step;
        
        AudioSource.PlayClipAtPoint(dashSFX, transform.position);
    }
}
