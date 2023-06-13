using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Canvas messagePrefab;
    
    Animator anim;
    float distanceToPlayer;
    bool messageShown = false;
    Canvas message;

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

        if(distanceToPlayer <= approachDistance && !FindObjectOfType<PlayerController>().sawMessage)
        {
            currentState = FSMStates.Wave;
        }
    }

    void UpdateWaveState()
    {
        anim.SetInteger("animState", 1);

        PlayMessage();
        Invoke("SetIdleState", 1.5f);
    }

    void UpdateIdleState()
    {
        anim.SetInteger("animState", 0);

        if(Input.GetKeyDown(KeyCode.X))
        {
            ExitMessage();
            currentState = FSMStates.Pace;
        }
    }

    void SetIdleState()
    {
        currentState = FSMStates.Idle;
    }

    void PlayMessage()
    {
        FindObjectOfType<LevelManager>().Pause();
        transform.LookAt(player.transform);
        if (!messageShown)
        {
            message = Instantiate(messagePrefab) as Canvas;
            messageShown = true;
            FindObjectOfType<PlayerController>().sawMessage = true;
        }
    }

    void ExitMessage()
    {
        FindObjectOfType<LevelManager>().UnPause();
        if (messageShown)
        {
            Destroy(message);
            messageShown = false;
        }
    }
}
