using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.Animations;
using System;


[Serializable]
public class AttackStats 
{
    public string attackName;      //should be same as attack animation name
    public float minDistance;
    public float maxDistance;
    public float maxAngle;
    public bool angleIsPositive;   //is the angle infront of enemy? If yes then check the bool true.
    public float timeStart;        //when should attack colliders be enabled
    public float attackTimeLenght; //hopw long should attack colliders stay on
}

public class EnemyStateMachine : MonoBehaviour
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

    //health & resistances
    [Tooltip("overall health, to resist raw / virus / vaccine damages")]
    public float health = 100; //overall health, to resist raw / virus / vaccine damages
    [Tooltip("overall value how much stun damage needs to build up before this gets stunned")]
    public float stunResistance; //overall value how much stun damage needs to build up before this gets stunned
    [Tooltip("to resist virus damage")]
    public float virusResistance; //to resist virus damage, do some math
    [Tooltip(" to resist vaccine damage,")]
    public float vaccineResistance; //to resist vaccine damage, do some math
    private float stunBuildUp = 0;

    //AttackStats is used to determinate when to use what attack. 
    //To get array lenght, call attackStats.Lenght
    //To access value, call attackStats.valueName etc.
    [Header("attack name, minDistance, maxDistance, maxAngle, angleIsPositive, timeStart, attackTimeLenght")]
    [SerializeField]
    public AttackStats[] attackStats;
    //Attack stuff
    float attackCount;


    void Start()
    {
        animator = GetComponent<Animator>(); //animator
        _navMeshAgent = GetComponent<NavMeshAgent>(); //navigation agent
        _destination = PlayerManager.instance.player.transform; //get destination from PlayerManager. This is the target
        //set animations as false 
        animator.SetBool("running", false);
        animator.SetBool("attacking", false);
        animator.SetBool("dead", false);

        //testing attack stats from inspector
        attackCount = attackStats.Length;
       // print(attackStats.Length);
       // Debug.Log(attackStats[0].attackName);
       // Debug.Log(attackStats[0].minDistance);
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
    }

    public void ApplyDamage(float[] damageStorage)
    {
        Debug.Log(this.gameObject.name + " took damage ");
        float stun = damageStorage[1]; //to resist stun damage
        float virusDamage = damageStorage[2] - virusResistance; //<-- you could do some cool math here
        float vaccineDamage = damageStorage[3] - vaccineResistance; 

        //no negative values allowed
        if (virusDamage < 0){ virusDamage = 0; }
        if (vaccineDamage < 0){ vaccineDamage = 0; }

        health = health - damageStorage[0] - virusDamage - vaccineDamage; //calculate health after raw / virus / vaccine damages
       
        if(health <= 0) //When enemy dies:
        {
            //dead animation
            animator.SetTrigger("dead1");
            //fade script ON
            FadeTransparent script =  GetComponentInChildren<FadeTransparent>();
            script.enabled = true;
            //collider OFF
            SphereCollider collider = GetComponent<SphereCollider>();
            collider.enabled = false;
            //Turn moving script OFF
            EnemyMove enemyMove = GetComponent<EnemyMove>();
            enemyMove.enabled = false;
            //Turn agent OFF
            _navMeshAgent.isStopped = true;
            //NavMeshAgent agent = GetComponent<NavMeshAgent>();
            //agent.enabled = false;
            //Destroy Rigidbody
            Rigidbody rig = GetComponent<Rigidbody>();
            Destroy(rig);

            StartCoroutine(DestroyThis(7)); //here is waitTime until destroy enemy
        }
        IEnumerator DestroyThis(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            Destroy(gameObject);
        }
        if (stun > 0 && health > 0)
        {
            stunBuildUp += stun;
            if (stunBuildUp >= stunResistance) //if stunBuildUp overflows, enemy gets stunned
            {
                //stun
                animator.SetTrigger("took damage");
                stunBuildUp -= stunResistance;
            }
        }
    }
}
