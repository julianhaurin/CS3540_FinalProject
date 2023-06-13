using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaterEnemyBehavior : MonoBehaviour
{
    
    public Transform playerTransform;
    public GameObject enemyAttack;

    public int collisionDamage = 100;
    public int hitDamage = 20;

    public int attackSpeed = 2;
    public int movementSpeed = 5;

    // controls attack ranges
    public float closeRange;
    public float midRange;
    public float longRange;
    public float idleRange;

    private Vector3 vecToPlayer;
    private Animator anim;
    private float lastAttackTime;
    private float meleeRange;

    enum EnemyStates {
      Waiting,
      LongRange,
      MidRange,
      CloseRange,

    }

    EnemyStates currentState;

    private AudioSource audioSource;
    
    void Start() {

      currentState = EnemyStates.Waiting;
      anim = GetComponent<Animator>();
      audioSource = GetComponent<AudioSource>();

      if (playerTransform == null) {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        vecToPlayer = transform.position - playerTransform.position;
      }

      lastAttackTime = Time.time;
      meleeRange = 2f;
    }

    void Update() {

      vecToPlayer = transform.position - playerTransform.position;
      currentState = determineEnemyState(vecToPlayer);

      switch(currentState) {
        case(EnemyStates.Waiting):
          updateWaitingState();
          break;
        case(EnemyStates.LongRange):
          updateLongState();
          break;
        case(EnemyStates.MidRange):
          updateMidState();
          break;
        case(EnemyStates.CloseRange):
          updateCloseState();
          break;
      }

      transform.LookAt(playerTransform);

    }

    // Each function controls the enemy based on its respective state
    private void updateWaitingState() {
      if (Time.time > lastAttackTime + attackSpeed) {
        anim.SetInteger("animState", 0);
      }
    }

    private void updateLongState() {
      anim.SetInteger("animState", 0);
      if (Time.time > lastAttackTime + attackSpeed + 2) {
        lastAttackTime = Time.time;
        anim.Play("Base Layer.SpellCast");
        
        Invoke("longRangeAttack", 1.5f);

      }
      
    }

    private void longRangeAttack() {
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up, transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(2, 0, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(-2, 0, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(4, 0, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(-4, 0, 0), transform.rotation);

      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(0, 2, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(2, 2, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(-2, 2, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(4, 2, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(-4, 2, 0), transform.rotation);

      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(0, -2, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(2, -2, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(-2, -2, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(4, -2, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(-4, -2, 0), transform.rotation);
    }

    private void updateMidState() {
      anim.SetInteger("animState", 0);
      if (Time.time > lastAttackTime + attackSpeed - 1) {
        lastAttackTime = Time.time;
        anim.Play("Base Layer.PunchRight");

        Invoke("midRangeAttack", 0.5f);
        
      }
    }

    private void midRangeAttack() {
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up, transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward * 2 + transform.up, transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward * 3 + transform.up, transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward * 4 + transform.up, transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward * 5 + transform.up, transform.rotation);

    }

    private void updateCloseState() {

      // enemy is close enough to attack
      if (vecToPlayer.magnitude <= meleeRange) {
        anim.SetInteger("animState", 0);

        if (Time.time > lastAttackTime + attackSpeed) { // melee attack
          lastAttackTime = Time.time;
          anim.Play("Base Layer.MeleeAttack_OneHanded");
          audioSource.Play();
          if (vecToPlayer.magnitude < meleeRange) {
            var playerHealth = other.GetComponent<Health>();
            playerHealth.TakeDamage(collisionDamage);
          };
          
        }

      // enemy moves towards players
      } else {
        anim.SetInteger("animState", 1);
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, movementSpeed * Time.deltaTime);

      }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            var playerHealth = other.GetComponent<Health>();
            playerHealth.TakeDamage(collisionDamage);

        } else if (other.CompareTag("Projectile")) {
            var enemyHealth = gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(hitDamage);

        }
    }

    // determines current enemy state - based on distance to player
    private EnemyStates determineEnemyState(Vector3 in_vecToPlayer) {

      EnemyStates returnState;
      float distanceToPlayer = in_vecToPlayer.magnitude;

      if (distanceToPlayer < closeRange) {
        returnState = EnemyStates.CloseRange;
      } else if (distanceToPlayer < midRange) {
        returnState = EnemyStates.MidRange;
      } else if (distanceToPlayer < longRange) {
        returnState = EnemyStates.LongRange;
      } else {
        returnState = EnemyStates.Waiting;
      }
      return returnState;
      
    }

    // private void Attack() {
    //     Instantiate(enemyAttack, transform.position + transform.forward + transform.up, transform.rotation);
    // }
}