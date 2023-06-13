using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public GameObject enemyAttack;
    public int collisionDamage = 100;
    public int hitDamage = 20;
    [Range(1, 20)]
    [Tooltip("How likely the enemy is to attack during each frame")]
    public int attackRate = 1;
    
    private int speed = 2;
    
    // private bool clockwise = true;
    
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<LevelManager>().isPaused)
        {

            if (Random.Range(0, 500) < attackRate)
            {
                Attack();
            } 

            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.LookAt(player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!FindObjectOfType<LevelManager>().isPaused)
        {
            if(other.CompareTag("Player"))
            {
                var playerHealth = other.GetComponent<Health>();
                playerHealth.TakeDamage(collisionDamage);
            } else if (other.CompareTag("Projectile"))
            {
                var enemyHealth = gameObject.GetComponent<Health>();
                enemyHealth.TakeDamage(hitDamage);
            }
        }
    }

    private void Attack()
    {
        Instantiate(enemyAttack, transform.position + transform.forward + transform.up, transform.rotation);
    }
}
