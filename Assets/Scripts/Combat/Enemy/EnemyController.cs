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
    public bool spawnable = false;

    //public HumanBodyBones bone;
    //public SphereCollider sphereCollider;

    NavMeshAgent agent;
    Animator animator;

    Vector3 startPoint;  // The enemy's starting position
    Vector3 patrolPoint;  // The current patrol point

    float timeDestinationReached;  // The time when the destination was reached
    float lastAttack;
    float waitTime = 4f;
    public float waitAttack = 2f;

    public GameObject prefab1;
    public GameObject prefab2;

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
        float distance = Vector3.Distance(target.position, transform.position);

        if(dead == false && distance <= lookRadius)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", true);
            Atack(distance);
            
        }
        else
        {
            animator.SetBool("IsRunning", false);

            Patrol();
        }
    }

    void Atack(float distanceFromTarget)
    {
        agent.SetDestination(target.position);

        animator.ResetTrigger("IsJumpAttackRange");
        animator.ResetTrigger("IsMeleeRange");

        if (distanceFromTarget <= jumpAttackRadius && distanceFromTarget > meleeRadius && Time.time >= lastAttack + waitAttack)
        {
            FaceTarget();
            //animator.SetBool("IsJumpAttackRange", true);
            animator.SetTrigger("IsJumpAttackRange");
            lastAttack = Time.time;
        }

        if (distanceFromTarget <= meleeRadius && Time.time >= lastAttack + waitAttack)
        {
            FaceTarget();
            //animator.SetBool("IsMeleeRange", true);
            animator.SetTrigger("IsMeleeRange");
            lastAttack = Time.time;
        }

    }

    void Patrol()
    {
        if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
        {
            // Enemy has a set destination and is still far from reaching it
            Debug.Log("Enemy has a destination set");

            checkForWall();

            if (timeDestinationReached < Time.time - 15f)
            {
                animator.SetBool("IsWalking", true);
                patrolPoint = GetRandomPatrolPoint();
                agent.SetDestination(patrolPoint);  
            }
        }
        else
        {
            // Enemy either doesn't have a destination or has reached it
            Debug.Log("Enemy doesn't have a destination set");

            animator.SetBool("IsWalking", true);
            Debug.Log("destination set");
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
        if(dead) return;
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

            if(spawnable)
            {
                // Spawn the first prefab
                GameObject instance1 = Instantiate(prefab1, transform.position, Quaternion.identity);
                instance1.GetComponent<EnemyController>().target = this.target;

                // Spawn the second prefab
                GameObject instance2 = Instantiate(prefab2, transform.position + new Vector3(2f, 0f, 0f), Quaternion.identity);
                instance2.GetComponent<EnemyController>().target = this.target;
            }

            StartCoroutine(DestroyAfterDelay(5f)); // Wait for 5 seconds before destroying
        } 

    }

    public void checkForWall()
    {
        // Get the direction of the enemy's movement
        Vector3 movementDirection = agent.velocity.normalized;

        // Cast a ray in the direction of movement
        RaycastHit hit;
        if (Physics.Raycast(transform.position, movementDirection, out hit, 1f))
        {
            // Check if the ray hits a wall (based on the collision layer you set)
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
            {
                // Wall detected, handle the situation (e.g., change patrol behavior, attack, etc.)
                Debug.Log("Wall detected!");

                // Example: Stop the enemy from moving
                agent.SetDestination(GetRandomPatrolPoint());
            }
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
