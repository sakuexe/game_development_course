using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MoveToTarget))]
public class EnemyLogic : MonoBehaviour
{
    private MoveToTarget moveComponent;
    public float enemySpeed = 3f;
    
    private Transform player;
    private EnemyState currentState;
    private Vector3 patrolPoint;
    private float idleTime;

    // For adjusting. How far detects player (now just orb shaped detection), how far will wander at max, idle min max values.
    public float detectionRange = 10f;
    public float patrolRange = 15f;
    public float idleTimeMin = 4f;
    public float idleTimeMax = 10f;

    private void Start()
    {
        //At the start (of this object's perspective) we fetch move component, set speed and find player from hierarchy / scene.
        moveComponent = GetComponent<MoveToTarget>();
        moveComponent.SetSpeed(enemySpeed);

        player = GameObject.FindWithTag("Player").transform;
        //Aaand set initial state.
        SetState(EnemyState.Patrolling);
    }

    private void Update()
    {
        //We check current state and handle it every frame.
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Patrolling:
                HandlePatrolState();
                break;
            case EnemyState.Chasing:
                HandleChaseState();
                break;
        }
    }

    private void SetState(EnemyState newState)
    {
        currentState = newState;


        //We could have these as their own functions, but for now, this is fine.

        if (newState == EnemyState.Idle)
        {
            idleTime = Random.Range(idleTimeMin, idleTimeMax); // Set random idle duration
        }
        else if (newState == EnemyState.Patrolling)
        {
            patrolPoint = GetRandomPatrolPoint(); // Set new patrol destination
            moveComponent.MoveTo(patrolPoint);
        }
    }

    private void HandleIdleState()
    {
        idleTime -= Time.deltaTime;
        if (idleTime <= 0)
        {
            SetState(EnemyState.Patrolling); // Switch to patrol after idle time
        }

        if (IsPlayerInRange())
        {
            SetState(EnemyState.Chasing);
        }
    }

    private void HandlePatrolState()
    {
        //We check a boolean from our move component.  If we've reached our patrol point, we switch to idle.
        if (moveComponent.HasReachedDestination())
        {
            SetState(EnemyState.Idle); // Arrived at patrol point, switch to idle
        }

        //We check if player is in range.  If so, we switch to chasing.
        if (IsPlayerInRange())
        {
            SetState(EnemyState.Chasing); // Run boy!
        }
    }

    private void HandleChaseState()
    {
        if (!IsPlayerInRange())
        {
            SetState(EnemyState.Patrolling); // Lost sight of player, returning to patrolling.
        }
        else
        {
            moveComponent.MoveTo(player.position); // Continue chasing player
        }
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) < detectionRange;
    }

    private Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolRange;
        NavMeshHit hit;

        //This line of code is part of Unity's NavMesh system and is used to find 
        //a valid position on the NavMesh (navigation mesh) that's closest to a given random point.
        NavMesh.SamplePosition(randomPoint, out hit, patrolRange, NavMesh.AllAreas);

        return hit.position;
    }
}