using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.Animations;

public class AnimationTestScript : MonoBehaviour
{
    //Used to test enemy animations. Not very good becauseit needs lots of manual work
    public bool running = false;
    public bool attacking = false;
    public bool taking_damage = false;
    public bool dead = false;

    NavMeshAgent _navMeshAgent; //navigation component
    Animator animator;
    float distance;
    //use  animator.SetBool("attacking", true);   to change parameters

    //get current speed
    private Vector3 previousPosition;
    public float curSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        animator.SetBool("running", false);
        animator.SetBool("attacking", false);
        animator.SetBool("dead", false);
    }

    void Update()
    {

        if (running == true)
        {
            attacking = false;
            dead = false;
            taking_damage = false;
            animator.SetBool("running", true);
        }
        if (dead == true)
        {
            running = false;
            attacking = false;
            taking_damage = false;
            animator.SetTrigger("dead1");
        }
        if (taking_damage == true)
        {
            running = false;
            attacking = false;
            animator.SetTrigger("took damage");
        }
        if (attacking == true)
        {
            running = false;
            dead = false;
            taking_damage = false;
            animator.SetTrigger("attack");
        }



    }
}
