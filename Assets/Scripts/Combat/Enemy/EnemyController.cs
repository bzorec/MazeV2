using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemyController : MonoBehaviour, IDamageable
{
    public float health;
    public Transform target;
    public float lookRadius = 10f;
    public float meleeRadius = 2f;
    public float jumpAttackRadius = 4f;
    public float patrolRadius = 5f;  // The radius around the enemy's starting position in which patrol points can be generated

    //public HumanBodyBones bone;
    //public SphereCollider sphereCollider;

    NavMeshAgent agent;
    Animator animator;

    Vector3 startPoint;  // The enemy's starting position
    Vector3 patrolPoint;  // The current patrol point

    float timeDestinationReached;  // The time when the destination was reached
    float lastAttack;
    float waitTime = 4f;
    float waitAttack = 2f;

    private bool dead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        startPoint = transform.position;
        patrolPoint = GetRandomPatrolPoint();
        timeDestinationReached = Time.time;
        lastAttack = Time.time;

        animator.SetBool("isDead", false);

    }


    void Update()
    {

        //animator.SetBool("IsRunning", true);
        float distance = Vector3.Distance(target.position, transform.position);

        if(dead == false && distance <= lookRadius)
        {
            animator.SetBool("IsWalking", false);
            Atack(distance);
            
        }
        else
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsMeleeRange", false);
            animator.SetBool("IsJumpAttackRange", false);

            if(Time.time >= timeDestinationReached + waitTime) {
                animator.SetBool("IsWalking", true);
                Patrol();
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }

        }





        /*if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();

                if (distance <= meleeRadius)
                {
                    animator.SetBool("IsMeleeRange", true);
                }
                else
                {
                    animator.SetBool("IsMeleeRange", false);
                }

                if (distance <= jumpAttackRadius && distance > meleeRadius)
                {
                    animator.SetBool("IsJumpAttackRange", true);
                }
                else
                {
                    animator.SetBool("IsJumpAttackRange", false);
                }
            }
            else
            {
                animator.SetBool("IsMeleeRange", false);
                animator.SetBool("IsJumpAttackRange", false);
                animator.SetBool("IsRunning", true);
            }
        }
        else
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsMeleeRange", false);
            animator.SetBool("IsJumpAttackRange", false);

            Patrol();
        }*/
    }

    void Atack(float distanceFromTarget)
    {
        animator.SetBool("IsRunning", true);
        agent.SetDestination(target.position);

        if (distanceFromTarget <= jumpAttackRadius && distanceFromTarget > meleeRadius && Time.time >= lastAttack + waitAttack)
        {
            FaceTarget();
            animator.SetBool("IsJumpAttackRange", true);
            lastAttack = Time.time;
        }
        else
        {
            animator.SetBool("IsJumpAttackRange", false);
        }

        if (distanceFromTarget <= meleeRadius && Time.time >= lastAttack + waitAttack)
        {
            FaceTarget();
            animator.SetBool("IsMeleeRange", true);
            lastAttack = Time.time;
        }
        else
        {
            animator.SetBool("IsMeleeRange", false);
        }
    }

    void Patrol()
    {
        if (agent.hasPath && agent.remainingDistance <= agent.stoppingDistance + 2)
        {
            // The agent has reached its destination
            Debug.Log("destination set");
            timeDestinationReached = Time.time;
            patrolPoint = GetRandomPatrolPoint();
            agent.SetDestination(patrolPoint);

        }
        else if (!agent.hasPath)
        {
            // The agent doesn't have a path
            Debug.Log("first destinatoin");
            patrolPoint = GetRandomPatrolPoint();
            agent.SetDestination(patrolPoint);

        }
        else
        {
            // The agent is still moving towards its destination
        }

        if(timeDestinationReached <= Time.time - 15f) {
            patrolPoint = GetRandomPatrolPoint();
            agent.SetDestination(patrolPoint);
        }
    }

    Vector3 GetRandomPatrolPoint()
    {
        // Generate a random direction and distance within the patrol radius
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;

        // Create the patrol point in the random direction from the enemy's starting position
        Vector3 patrolPoint = startPoint + randomDirection;

        // Ensure the patrol point is on the NavMesh
        NavMesh.SamplePosition(patrolPoint, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas);
        patrolPoint = hit.position;

        return patrolPoint;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void TakeDamage(float damage)
    {
        if(health - damage <= 0)
        {
            animator.SetBool("isDead", true);
            dead = true;
            agent.isStopped = true;
        }

        health -= damage;
        animator.SetTrigger("damageTaken");        
        health -= damage;

        if (health <= 0) {

            StartCoroutine(DestroyAfterDelay(5f)); // Wait for 5 seconds before destroying
        } 

    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
