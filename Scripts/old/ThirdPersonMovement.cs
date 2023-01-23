using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    //code from Brackeys (Third person movement in Unity)
    //Character movement with angle direction
    public CharacterController controller;
    public Transform cam; //This is the Main Camera, not the cinemachine camera
    Animator animator;
    AnimatorStateInfo animatorInfo; 

    private float moveSpeed; //this value changes
    public float speed = 5f;
    public float runSpeed = 9f;
    private float curSpeed; //current speed
    private Vector3 previousPosition;
    
    //smoothen turning when direction changes
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Character_hit_detection character_hit_detection;

    private void Start()
    {
        character_hit_detection = GetComponent<Character_hit_detection>();
        if (animator = GetComponentInChildren<Animator>())
        {
            //success
        }
        else { animator = GetComponent<Animator>(); }
        moveSpeed = speed;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //get current speed
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;

        if (direction.magnitude >= 0.01f)
        {
            animator.SetBool("running", true);

            //take tangent angle + camera angle
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //smoothen turning
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //move character WASD
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("running", false);
        }
        if (controller.isGrounded == true) //if player is on ground
        {
            if (Input.GetAxis("Jump") > 0 && curSpeed > 0.1f) //jump
            {
                //jump animation that moves character

            }
            if (Input.GetAxis("Run") > 0) //run by increasing speed
            {           
                moveSpeed = runSpeed;
            }
            else 
            {
                moveSpeed = speed; //if player is not running
            }
        }

        //test attack 
        if (Input.GetButtonDown("Attack1")){
            animator.SetTrigger("attack1");
        }
        else if(Input.GetButtonDown("Attack2")){
            animator.SetTrigger("attack2");
        }
        //death
        if(character_hit_detection.health <= 0)
        {
            animator.SetBool("dead", true);
        }
    }
}
