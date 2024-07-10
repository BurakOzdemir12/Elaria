using Invector;
using Micosmo.SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : NetworkBehaviour
{
    public NavMeshAgent agent;

    private Transform player; // Reference to the instantiated player's transform

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    private bool patrollingStopped = false;

    //Attacking
    public float timeBetweenAttacks;
    public float timeBetweenRangeAttacks;
    bool alreadyAttacked;
    bool alreadyRangeAttacked;
    public GameObject projectile;
    


    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    //animation
    Animator animator;
    //sensor
    public Sensor TargetSensor;
    public float projectileSpeed;
    public float projectileLifetime;
    public Transform handTransform;
    public float projectileLaunchDelay; // Delay before launching projectile
    private Transform targetHeadTransform;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    private void Awake()
    {
        /*   GameObject playerInstance = Instantiate(playerPrefab.gameObject);
           player = playerInstance.transform;*/
        this.enabled = true;


        
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Sensor for range attacking

/*        targetHeadTransform = GameObject.Find("HumanMaleCharacterThirdPerson").transform;
*//*        player = GameObject.Find("HumanMaleCharacterThirdPerson").transform;
*/
        if (player == null)
        {


            FindPlayer();
            if (player == null) return; // If no player is found, do nothing
        }


        var target = TargetSensor.GetNearestDetection();

            if (target != null && !playerInAttackRange && !playerInSightRange)
            {
                FireAtTarget(target.transform);
                Debug.Log("Target detected: " + target.transform.name);
            }
       

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange ) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!patrollingStopped)
        {
            
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
        }

    }
    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        GameObject targetHeadTransformm = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            targetHeadTransform = targetHeadTransformm.transform;
        }
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        
    }
    
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            animator.SetBool("Attack", true);
            animator.SetBool("RangeAttack", false);


            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }

    private void FireAtTarget(Transform target)
    {
        patrollingStopped = true;
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyRangeAttacked)
        {
            animator.SetBool("RangeAttack", true);
            alreadyRangeAttacked = true;

            StartCoroutine(LaunchProjectile(target));
            Invoke(nameof(ResetRangeAttack), timeBetweenRangeAttacks);
        }

    }
    private IEnumerator LaunchProjectile(Transform target)
    {
        // Wait for the animation event to trigger the projectile launch
        yield return new WaitForSeconds(projectileLaunchDelay); // Use the inspector variable for delay

        Vector3 targetPosition = targetHeadTransform != null ? targetHeadTransform.position+Vector3.up*5f : target.position;
        GameObject proj = Instantiate(projectile, handTransform.position, Quaternion.identity);
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        Vector3 direction = (targetPosition - handTransform.position).normalized;
        rb.velocity = direction * projectileSpeed;
        SoundManager.instance.PlayProjectileRock();

        Destroy(proj, projectileLifetime);

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void ResetRangeAttack()
    {
        alreadyRangeAttacked = false;
        patrollingStopped = false;

    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
/* Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
       rb.AddForce(target.forward * 32f, ForceMode.Impulse);
       rb.AddForce(target.up * 8f, ForceMode.Impulse);
*/