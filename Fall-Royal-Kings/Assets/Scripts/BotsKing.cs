using System.Collections;
using System.Collections.Generic;
using SensorToolkit;
using UnityEngine.AI;
using UnityEngine;

public class BotsKing : MonoBehaviour
{
    // navmesh agent
    private NavMeshAgent aiAgent;
    public float currentHealth = 100;

    // sensor
    public RangeSensor sensor;

    // player
    public Transform botTarget;

    // attack settings
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackPower;
    public LayerMask enemyLayers;


    // animation component
    public WarriorAnimationBehaviour playerAnimationBehaviour;
    bool attackManager = true;


    public GameObject introParticle;

    // states
    public enum State
    {
        Idle,
        AttackPlayer,
        GoalChase,
        Dead
    }

    public State state;

    private void Awake()
    {
        // aiAgent
        aiAgent = GetComponent<NavMeshAgent>();
        aiAgent.speed = Random.Range(1F, 2F);
        attackPower = Random.Range(1, 20);

        // current state
        state = State.Idle;


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    public void FaceTarget()
    {
        Vector3 direction = (botTarget.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }


    public void GotAttacked(int damagePoints)
    {

        currentHealth -= damagePoints;
        // Debug.Log(currentHealth);

        playerAnimationBehaviour.PlayHitAnimation();

        if (currentHealth <= 0)
        {
            state = State.Dead;
            playerAnimationBehaviour.PlayDeathAnimation();

        }

    }

    public void Attack()
    {

        if (currentHealth > 0)
        {
           
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.name != gameObject.name)
                {
                    try
                    {
                     
                        enemy.GetComponent<BotsKing>().GotAttacked(attackPower);
                        playerAnimationBehaviour.PlayAttackAnimation();

                    }
                    catch (System.Exception e)
                    {
                        // Debug.Log(e);
                    }

                    try
                    {

                        enemy.GetComponent<PlayerController>().GotAttacked(attackPower);
                        playerAnimationBehaviour.PlayAttackAnimation();

                    }
                    catch (System.Exception e)
                    {
                        // Debug.Log(e);
                    }

                }
              
            }

        }

    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ActivationRoutine");   
    }

    void AttackChase(GameObject detectedObject)
    {
        botTarget = detectedObject.transform;
        state = State.AttackPlayer;
    }

    IEnumerator WaitCoroutine()
    {
        attackManager = false;

        float seconds = Random.Range(1f,1.5f);

        yield return new WaitForSeconds(seconds);

        attackManager = true;
    }

    private IEnumerator ActivationRoutine()
    {
        
        //Turn My game object that is set to false(off) to True(on).
        introParticle.SetActive(true);

        //Turn the Game Oject back off after 1 sec.
        yield return new WaitForSeconds(5);

        //Game object will turn off
        introParticle.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

        if (state != State.Dead)
        {
            var detected = sensor.GetNearest();

            if (detected)
            {
                AttackChase(detected);

            }
            else
            {
                botTarget = GameObject.FindGameObjectWithTag("EndPoint").transform;
                state = State.GoalChase;
            }

        }
       
        switch (state)
        {
            case State.AttackPlayer:

                float distance = Vector3.Distance(botTarget.position, transform.position);

                aiAgent.SetDestination(botTarget.position);

                FaceTarget();

                playerAnimationBehaviour.PlayRunAnimation();

                if (distance <= aiAgent.stoppingDistance)
                {

                    if (attackManager == true)
                    {
                        FaceTarget();
                        Attack();
                        StartCoroutine(WaitCoroutine());

                    }

                }

                break;

            case State.GoalChase:

                float goaldistance = Vector3.Distance(botTarget.position, transform.position);
                aiAgent.SetDestination(botTarget.position);

                FaceTarget();
                playerAnimationBehaviour.PlayRunAnimation();

                if (goaldistance <= aiAgent.stoppingDistance)
                {
                    FaceTarget();
                    state = State.Idle;
                }

                break;

            case State.Idle:
                playerAnimationBehaviour.PlayIdleAnimation();
                break;

            case State.Dead:

                aiAgent.enabled = false;
                gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                break;

        }

    }

}
