using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class SCR_Enemy_Range : MonoBehaviour {
    public Transform[] patrolPoints; 
    public float detectionRadius = 10f;
    public int damage;
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float fireRate = .5f;
    public float nextAttackTime = 0f;
    public float projectileSpeed = 100f;
    private NavMeshAgent agent; 
    private int currentPatrolIndex = 0; 
    private Transform player; 
    private bool playerDetected = false;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        MoveToNextPatrolPoint();
    }

    private void Update() {
        if (playerDetected)
        {
            agent.isStopped = true;
            if (player != null && Time.time >= nextAttackTime)
            {
                Debug.Log("Prepare to Fire");
                Attack();
                nextAttackTime = Time.time + fireRate;
            }
        }
        else
        {
            agent.isStopped = false;
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                MoveToNextPatrolPoint();
            }
        }

        Collider[] playerInRange = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider col in playerInRange)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("Player Detected! Stopping Movement.");
                player = col.transform;
                playerDetected = true;
                return;
            }
        }

        // Reset if player is not found
        playerDetected = false;
    }

    private void Attack() {
        Debug.Log("Fire!");
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        SCR_ProjectileBehavior projectileScript = projectile.GetComponent<SCR_ProjectileBehavior>();
        projectileScript.SetDamage(damage);
        Vector3 targetPosition = player.position + Vector3.up * .75f;
        Vector3 direction = (targetPosition - firePoint.position).normalized;
        projectile.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileSpeed, ForceMode.Impulse);

    }

    
    private void MoveToNextPatrolPoint() {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
