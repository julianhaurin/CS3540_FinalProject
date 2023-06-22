using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// **** ADD PATROL STATE
// **** CHECK IF PLAYER IS BELOW Y LEVEL / has fallen off arena
// add enemy AI sight?

public class EnemyAI : MonoBehaviour
{
    
    public static int enemyHealth;
    public int enemyStartingHealth;

    public GameObject player;
    public GameObject enemyAttack;

    private UnityEngine.AI.NavMeshAgent agent;

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

    private GameObject[] patrolPositions;
    private Transform nextDestination;
    private int patrolPositionIndex;

    private float fieldOfView = 45f;

    enum EnemyStates {
      Patrol,
      LongRange,
      MidRange,
      CloseRange,

    }

    EnemyStates currentState;

    private AudioSource audioSource;
    
    void Start() {

      enemyHealth = enemyStartingHealth;

      currentState = EnemyStates.Patrol;
      agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
      anim = GetComponent<Animator>();
      audioSource = GetComponent<AudioSource>();

      patrolPositions = GameObject.FindGameObjectsWithTag("PatrolPosition");
      patrolPositionIndex = 0;
      nextDestination = patrolPositions[patrolPositionIndex].transform;

      if (player == null) {
        player = GameObject.FindGameObjectWithTag("Player");
      }

      vecToPlayer = transform.position - player.transform.position;

      lastAttackTime = Time.time;
      meleeRange = 3f;
      agent.stoppingDistance = meleeRange;

    }

    void Update() {

      if (enemyHealth <= 0) {
        Debug.Log("enemy dies");
        Destroy(gameObject);
        FindObjectOfType<LevelManager>().LevelBeat();
        return;
      } 

      if (FindObjectOfType<LevelManager>().isPaused) return;

      vecToPlayer = transform.position - player.transform.position;
      
      if (currentState == EnemyStates.Patrol) {
        updatePatrolState();
      } else {
        currentState = determineEnemyState(vecToPlayer);

        switch(currentState) {
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
      }
    }

    // Each function controls the enemy based on its respective state
    private void updateWaitingState() {
      if (Time.time > lastAttackTime + attackSpeed) {
        anim.SetInteger("animState", 0);
      }
      transform.LookAt(player.transform);
    }

    private void updatePatrolState() {

      anim.SetInteger("animState", 1);

      if ((transform.position - nextDestination.position).magnitude <= meleeRange) {
        patrolPositionIndex++;
        if (patrolPositionIndex >= patrolPositions.Length) patrolPositionIndex = 0;
        nextDestination = patrolPositions[patrolPositionIndex].transform;

      }
      agent.SetDestination(nextDestination.position);
      transform.LookAt(nextDestination);
      
      if (IsPlayerInClearFOV()) {
        // Debug.Log("PLAYER SEEN");
        currentState = determineEnemyState(vecToPlayer);
      }

    }

    private void updateLongState() {
      anim.SetInteger("animState", 0);
      if (Time.time > lastAttackTime + attackSpeed + 2) {
        lastAttackTime = Time.time;
        anim.Play("Base Layer.SpellCast");
        
        Invoke("longRangeAttack", 1.5f);

      }
      transform.LookAt(player.transform);
      
    }

    private void longRangeAttack() {
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up, transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(2f, 0, 0), transform.rotation);
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up + new Vector3(-2f, 0, 0), transform.rotation);
    }

    private void updateMidState() {
      anim.SetInteger("animState", 0);
      if (Time.time > lastAttackTime + attackSpeed - 1) {
        lastAttackTime = Time.time;
        anim.Play("Base Layer.PunchRight");

        Invoke("midRangeAttack", 0.5f);
        
      }
      transform.LookAt(player.transform);
    }

    private void midRangeAttack() {
      Instantiate(enemyAttack, transform.position + transform.forward + transform.up, transform.rotation);

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
            player.GetComponent<Health>().TakeHealth(collisionDamage);

          };
          
        }

      // enemy moves towards players
      } else {
        anim.SetInteger("animState", 1);
        agent.SetDestination(player.transform.position);
        // transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, movementSpeed * Time.deltaTime);

      }

      CustomLookAt(player);

    }

    bool IsPlayerInClearFOV() {

      RaycastHit hit;
      GameObject enemeyEyes = transform.GetChild(0).gameObject;
      
      Vector3 directionToPlayer = player.transform.position - enemeyEyes.transform.position;

      

      if (Vector3.Angle(directionToPlayer, enemeyEyes.transform.forward) <= fieldOfView) {
        // Debug.Log("test");

        // checks if raycast between player and enemy is unobstructed
        if (Physics.Raycast(enemeyEyes.transform.position, directionToPlayer, out hit, 1000)) {
          // Debug.Log(hit.collider);
          if (hit.collider.CompareTag("Player")) return true;

        }
      }

      
      return false;

    }

    private void OnTriggerEnter(Collider other) {
        // if(other.CompareTag("Player")) {
        //     var playerHealth = other.GetComponent<Health>();
        //     playerHealth.TakeDamage(collisionDamage);

        if (other.CompareTag("Projectile")) {
            enemyHealth -= 10;
            Debug.Log(enemyHealth);

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
        returnState = EnemyStates.Patrol;
      }
      return returnState;
      
    }

    private void CustomLookAt(GameObject target) {
      var lookPos = target.transform.position - transform.position;
      Quaternion lookRot = Quaternion.LookRotation(lookPos);
      lookRot.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, lookRot.eulerAngles.y, transform.rotation.eulerAngles.z);
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 3);

    }

    // private void Attack() {
    //     Instantiate(enemyAttack, transform.position + transform.forward + transform.up, transform.rotation);
    // }
}