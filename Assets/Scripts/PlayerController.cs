using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector3 input, moveDirection;
    public float speed = 10f;

    public float dashSpeedMultiplier = 5f;
    public float dashRate = 3f;
    public float dashDuration = 1f;

    float nextDashTime = 0f;
    float lastDashTime = 0f;

    public float jumpHeight = 7f;
    public float gravity = 9.8f;
    public float airControl = 7.0f;

    public AudioClip dashSFX;
    public bool sawMessage;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<LevelManager>().isPaused)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");


            //make sure your updating current location, not starting over
            //normalize to make sure when pressing both youre not moving faster than just 1 direction
            input = (transform.right * moveHorizontal + transform.forward* moveVertical).normalized;

            input *= speed;
            if (controller.isGrounded)
            {
                moveDirection = input;

                if (Input.GetButton("Jump") && SceneManager.GetActiveScene().name == "WaterArena")
                {
                    moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                } 

                // DASHING FUNCTIONALITY
                else if (Input.GetKey(KeyCode.E) && SceneManager.GetActiveScene().name == "AirArena") {

                    AudioSource.PlayClipAtPoint(dashSFX, transform.position);

                    // pressing left shift performs a dash (multiplies speed by specific amount)
                    if (Time.time < lastDashTime + dashDuration) {
                        // increases player speed for a specific amount of time (dashDuration)
                        moveDirection.x *= dashSpeedMultiplier;
                        moveDirection.z *= dashSpeedMultiplier;
                        Debug.Log("Dashing!");

                    } else if (Time.time > nextDashTime) {
                        // handles the first dash press after a cooldown
                        moveDirection.x *= dashSpeedMultiplier;
                        moveDirection.z *= dashSpeedMultiplier;
                        
                        // initializes time variables necessary to calculate cooldown and duration
                        lastDashTime = Time.time;
                        nextDashTime = Time.time + dashRate;

                    } // otherwise, the dash cooldown is in effect -> no dash


                }
                else
                {
                    moveDirection.y = 0.0f;
                }
            }
            else
            {

                input.y = moveDirection.y;
                moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
            }

            if (moveDirection.x != 0 || moveDirection.z != 0)
            {
                sawMessage = false;
            }

            moveDirection.y -= gravity * Time.deltaTime;

            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
            moveDirection.x = 0;
            moveDirection.z = 0;

            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}
