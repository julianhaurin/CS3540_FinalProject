using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAI : MonoBehaviour
{

    public enum FSMStates
    {
        Idle,
        Pace,
        Wave
    };

    public FSMStates currentState;

    public float approachDistance = 5;
    public GameObject player;
    
    Animator anim;
    float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        currentState = FSMStates.Pace;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch(currentState)
        {
            case FSMStates.Pace:
                UpdatePaceState();
                break;
            case FSMStates.Wave:
                UpdateWaveState();
                break;
            case FSMStates.Idle:
                UpdateIdleState();
                break;
        }
        
    }

    void UpdatePaceState()
    {
        anim.SetInteger("animState", 2);

        if(distanceToPlayer <= approachDistance)
        {
            currentState = FSMStates.Wave;
        }
    }

    void UpdateWaveState()
    {
        anim.SetInteger("animState", 1);


        Invoke("SetIdleState", 1.5f);
    }

    void UpdateIdleState()
    {
        anim.SetInteger("animState", 0);

        if(distanceToPlayer > approachDistance)
        {
            currentState = FSMStates.Pace;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentState = FSMStates.Pace;
        }
    }

    void SetIdleState()
    {
        currentState = FSMStates.Idle;
    }
}
