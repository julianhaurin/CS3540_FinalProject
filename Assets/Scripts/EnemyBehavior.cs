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

    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void Attack()
    {
        Instantiate(enemyAttack, transform.position + transform.forward + transform.up, transform.rotation);
    }
}
