using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class NPCMovement : MonoBehaviour 
{
    public NavMeshAgent agent;
    public Animator animator; 
    public float range; 
    public Transform centrePoint; 

    [Header("Movement Settings")]
    public float movementSpeed = 3.0f; 
    public float waitTime = 10.0f;     
    
    private float waitCounter;
    private bool isWaiting;
    public bool canMove = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = movementSpeed;
        
        waitCounter = waitTime;
    }

    void Update()
    {
        if (!canMove)
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            return;
        }

        agent.isStopped = false;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitCounter = waitTime;
            }

            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
            }
            else
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point))
                {
                    agent.SetDestination(point);
                    isWaiting = false;
                }
            }
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        Vector2 velocity = agent.velocity;
        bool moving = velocity.sqrMagnitude > 0.01f;

        animator.SetBool("isWalking", moving);

        if (moving)
        {
            Vector2 direction = velocity.normalized;
            animator.SetFloat("InputX", direction.x);
            animator.SetFloat("InputY", direction.y);
            
            animator.SetFloat("LastInputX", direction.x);
            animator.SetFloat("LastInputY", direction.y);
        }
        else
        {
            animator.SetFloat("LastInputX", 0);
            animator.SetFloat("LastInputY", -1);
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector2 randomPoint2D = (Vector2)center + Random.insideUnitCircle * range;
        Vector3 randomPoint = new Vector3(randomPoint2D.x, randomPoint2D.y, center.z);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
        { 
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}