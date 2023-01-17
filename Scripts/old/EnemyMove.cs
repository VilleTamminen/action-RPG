using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.Animations;

public class EnemyMove : MonoBehaviour
{
    //enemy moves to target destination by using nav mesh agent. It does not lose player after that.
    //destination is fecthed from PlayerManager Singleton
    //navmesh stoppingDistance can be altered in navMeshAgent component

    //how enemy notices player
    Transform _destination; //player transform is get from Playermanager
    NavMeshAgent _navMeshAgent; //navigation component 
    [Tooltip("how close can target be until noticed from front of enemy")]
    public float lookRadius = 10f;
    [Tooltip("How close target can come from behind until noticed")]
    public float proximityNotice = 3f; //how close player can go behind enemy until noticed
    [Tooltip("Full angle of view, left & right summed together")]
    public float fieldOfViewAngle = 140f; //will get halved later
    [Tooltip("how high eyes are, needed for raycast point")]
    public float eyeLevelHeight = 1f; //height of eyes so that raycast will be more realistic
    public bool playerInSight; //if for some reason I need enemy to stop following payer, make this false
    private SphereCollider col;

    //find angle to player
    private float angle = 0f;
    private Vector3 direction;

    //Raycast ignore layerMask
    int layerMask = 1 << 9; //enemies are on layerMask 9.

    //Animation
    //use    animator.SetBool("attacking", true); & animator.SetTrigger("attack");  to change parameters
    Animator animator;
    AnimatorStateInfo animatorInfo;
    public bool running = false;
    public bool attacking = false;
    public bool dead = false;

    //get current speed with nav mesh agent
    private float startSpeed; //save value of starting speed of nav mesh agent
    private Vector3 previousPosition;
    public float curSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        _destination = PlayerManager.instance.player.transform; //get destination from PlayerManager
        startSpeed = _navMeshAgent.speed; //might be needed later

        animator.SetBool("running", false);
        animator.SetBool("attacking", false);
        animator.SetBool("dead", false);

        //error check
        if (_navMeshAgent == null)
        {
            Debug.LogError("the nav mesh agent component is not attached to " + gameObject.name);
        }
    }

    void FixedUpdate()
    {
        //angle for field of view check. Angle is never greater than 180.
        direction = _destination.position - transform.position;
        // sign = (direction.y >= 0) ? 1 : -1;
        // offset = (sign >= 0) ? 0 : 360;
        angle = Vector3.Angle(transform.forward, direction);
        //distance to player
        float distance = Vector3.Distance(_destination.position, transform.position);
        
        //get current speed with nav mesh agent
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;
        animator.SetFloat("speed", curSpeed); //update speed for possible needs, like blend wallking/running animations

        if (playerInSight == false) //search for Player (distance, angle, raycast)
        {
            if (distance <= lookRadius) //Check is player close enough
            {
                if (angle <= fieldOfViewAngle * 0.5f) //fov must be halved, because of angle. Check is player in fov
                {
                    //Raycast to player, to see if player is visible and not behind wall
                    RaycastHit hit;
                    //invert layerMask so tit ignores enemies
                    layerMask = ~layerMask;
                    //increase y axis with eyeLevelHeight
                    Vector3 tr = new Vector3(transform.position.x, transform.position.y + eyeLevelHeight, transform.position.z);
                    if (Physics.Raycast(tr, direction * lookRadius, out hit, Mathf.Infinity, layerMask))
                    {
                        if (hit.collider.tag.Contains("Player")) //if it sees player
                        {
                            //if for some reason I need enemy to stop following payer, disable playerInSight
                            playerInSight = true;
                        }
                    }
                }
            }
            if (distance <= proximityNotice) //if player is too close to enemy, even if behind back
            {
                playerInSight = true;
            }
        }
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0); //for getting animator "Attacks" sub-state layer index, for tags and stuff
        //stop, attack and face target
        if (distance <= _navMeshAgent.stoppingDistance && animatorInfo.IsTag("Stopped") == false)
        {
            //face target
            FaceTarget();
        }        

        //ANIMATION STATES ------------------------------------------------------------      
        //Tags: Stopped, Free
        /*
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0); //for getting animator state tags
        if(animatorInfo.IsTag("Stopped")) //stop movement if tag is Stopped
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
          else
          {
              _navMeshAgent.isStopped = false;
          }
        
        if (curSpeed > 0.01)
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
        */


        if (playerInSight == true && attacking == false) //follow player
        {
            SetDestination();
        }
        if (dead == true)
        {
            dead = false;
            running = false;
            attacking = false;
            animator.SetBool("attacking", false);
            animator.SetBool("running", false);
            animator.SetFloat("speed", 0);
            animator.SetTrigger("dead1");
        }
      /*  if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) //enemy will stay dead?
        {
            animator.enabled = false;
        }*/

    }

    void FaceTarget()
    {
        //not necessarily needed, code from Brackeys
        if (_destination != null)
        {
            Vector3 direction = (_destination.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); //by increasing last float, you increase turn speed
        }
    }

    private void SetDestination()
    {
        if (_destination != null)
        {
           // animator.SetBool("no target", false);
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }

    }

    void OnDrawGizmosSelected() //draws colored spehers to help understand different radius values in editor.
    {
        //draws red circle around it to help understand lookRadius in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        //draws yellow circle around head to help see correct eyeLevelHeight in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + eyeLevelHeight, transform.position.z), 1.5f);
        //draw stopping distance
        Gizmos.color = Color.blue;
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        Gizmos.DrawWireSphere(transform.position, _navMeshAgent.stoppingDistance);
    }
}
