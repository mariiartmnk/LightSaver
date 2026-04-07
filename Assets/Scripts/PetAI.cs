
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PetAI : MonoBehaviour
{
    public enum PetState {Waiting, Following, Guiding}

    [Header("Settings")]
    [SerializeField] private PetState currentState = PetState.Waiting;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float followDistance = 1.5f;
    [SerializeField] private float interactDistance = 1.5f;
    [SerializeField] private float hintDelay = 10f;
    [SerializeField] private AudioClip meowSFX;
    private AudioSource localSource;

    private NavMeshAgent agent;

    [Header("References")]
    private Transform player;
    private Rigidbody2D rb;
    private LightBulbInteraction targetBulb;
    private float hintTimer;
    private bool hasTalkedToPlayer = false;

    [SerializeField] private GameObject interactUI;

    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = moveSpeed;

        localSource = GetComponent<AudioSource>();
        if(localSource == null) localSource = gameObject.AddComponent<AudioSource>();

        localSource.clip = meowSFX;
        localSource.loop = true;
        localSource.playOnAwake = false;
        localSource.spatialBlend = 1.0f;
        localSource.minDistance = 1f;
        localSource.maxDistance = 10f;
        localSource.rolloffMode = AudioRolloffMode.Linear;

        if(interactUI != null)
        {
            interactUI.SetActive(false);
        }

        if(PlayerMovement.Instance != null)
        {
            player = PlayerMovement.Instance.transform;
        }        
    }

    void Update()
    {
        HandleInteractionUI();

        if(!hasTalkedToPlayer) return;

        if(currentState == PetState.Following)
        {
            CheckForStruggle();
        }
    }

    void FixedUpdate()
    {
        if(!hasTalkedToPlayer) return;

        if(currentState == PetState.Following)
        {
            FollowPlayer();
        }
        else if(currentState == PetState.Guiding)
        {
            GuideToBulb();
        }
    }

    private void HandleInteractionUI()
    {
        if(hasTalkedToPlayer || player == null || interactUI == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if(distance <= interactDistance)
        {
            interactUI.SetActive(true);
        }
        else
        {
            interactUI.SetActive(false);
        }
    }

    public void TalkToPet()
    {
        if (!hasTalkedToPlayer)
        {
            hasTalkedToPlayer = true;
            currentState = PetState.Following;
            hintTimer = 0;
            if(interactUI != null)
                interactUI.SetActive(false);
        }
    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if(distance > followDistance)
        {
            /*Vector2 direction = (Vector2)player.position - rb.position;
            direction.Normalize();
            rb.MovePosition(rb. position + direction * moveSpeed * Time.fixedDeltaTime);*/
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            //rb.linearVelocity = Vector2.zero;
            agent.isStopped = true;
        }
    }

    private void CheckForStruggle()
    {
        hintTimer += Time.deltaTime;

        if(hintTimer >= hintDelay)
        {
            FindClosestBulb();
        }
    }

    private void FindClosestBulb()
    {
        LightBulbInteraction[] allBulbs = Object.FindObjectsByType<LightBulbInteraction>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        List<LightBulbInteraction> brokenBulbs = new List<LightBulbInteraction>();
        foreach(LightBulbInteraction b in allBulbs)
        {
            if(!b.IsFixed) brokenBulbs.Add(b);
        }


        if(brokenBulbs.Count > 0)
        {
            targetBulb = brokenBulbs.OrderBy(b => Vector2.Distance(transform.position, b.transform.position)).First();
            
            currentState = PetState.Guiding;
            hintTimer = 0;

            if(localSource != null && !localSource.isPlaying)
                localSource.Play();
        }
    }

    private void GuideToBulb()
    {
        if(targetBulb == null || targetBulb.IsFixed)
        {
            currentState = PetState.Following;
            agent.stoppingDistance = followDistance;

            if(localSource != null)
            {
                localSource.Stop();
            }
            return;
        }

        agent.isStopped = false;
        agent.stoppingDistance = 0.5f;
        agent.SetDestination(targetBulb.transform.position);

        /*float distanceToBulb = Vector2.Distance(transform. position, targetBulb.transform.position);

        if(distanceToBulb > 0.5f)
        {
            Vector2 direction = (Vector2)targetBulb.transform.position - rb.position;
            direction.Normalize();
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }*/
    }
}
