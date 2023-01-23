using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//unfinished, made by copying enemy state machine. must change animation names and their activations...
public class EnemyMovement : MonoBehaviour
{
    //Placed on enemy. Handles animation, health, death, attack determination.
    NavMeshAgent _navMeshAgent; //navigation component
    Transform _destination; //player transform
    Animator animator;
    AnimatorStateInfo animatorInfo;

    //distance to target
    float distance;
    //get current speed
    private Vector3 previousPosition;
    protected float curSpeed;
    //get angle
    private float angle = 0f;
    private Vector3 direction;

    private Character_hit_detection character_hit_detection;

    void Start()
    {
        character_hit_detection = GetComponent<Character_hit_detection>();
        animator = GetComponent<Animator>(); //animator
        _navMeshAgent = GetComponent<NavMeshAgent>(); //navigation agent
        _destination = PlayerManager.instance.player.transform; //get destination from PlayerManager. This is the target
        //set animations as false 
        animator.SetBool("running", false);
      //  animator.SetTrigger("attack1", false);
        animator.SetBool("dead", false);

    }

    void Update()
    {
        if (_destination != null) //if target exists
        {
            //get speed
            Vector3 curMove = transform.position - previousPosition;
            curSpeed = curMove.magnitude / Time.deltaTime;
            previousPosition = transform.position;
            //get angle
            direction = _destination.position - transform.position;
            angle = Vector3.Angle(transform.forward, direction);
            //get distance to player
            float distance = Vector3.Distance(_destination.position, transform.position);


            animatorInfo = animator.GetCurrentAnimatorStateInfo(0); //for getting animator state tags
            if (animatorInfo.IsTag("Dead") == false) //if enemy is alive
            {
                if (animatorInfo.IsTag("Stopped")) //stop movement if tag is Stopped
                {
                    _navMeshAgent.isStopped = true;
                }
                if (animatorInfo.IsTag("Free")) //free movement if tag is Free
                {
                    _navMeshAgent.isStopped = false;
                }
                if (distance > _navMeshAgent.stoppingDistance && animatorInfo.IsTag("Stopped") == false) //if target is further than stopping distance and no attack is perfromed, this can move
                {
                    _navMeshAgent.isStopped = false;
                    animator.SetBool("running", true);
                }

                //set running bool
                if (curSpeed > 0.1)
                {
                    animator.SetBool("running", true);
                }
                if (curSpeed < 0.01)
                {
                    animator.SetBool("running", false);
                }
                if (distance <= _navMeshAgent.stoppingDistance && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && angle <= 35)
                {
                    animator.SetTrigger("attack");
                }
            }
        }
        if (character_hit_detection.health <= 0)
        {
            //dead animation
            animator.SetTrigger("dead1");
            //fade script ON
            FadeTransparent script = GetComponentInChildren<FadeTransparent>();
            script.enabled = true;
            //collider OFF
            Collider collider = GetComponent<Collider>();
            collider.enabled = false;
            //Turn moving  OFF
            _destination = null;
            //Turn agent OFF
            _navMeshAgent.isStopped = true;
            //NavMeshAgent agent = GetComponent<NavMeshAgent>();
            //agent.enabled = false;
            //Destroy Rigidbody
            Rigidbody rig = GetComponent<Rigidbody>();
            Destroy(rig);
        }
    }
}
