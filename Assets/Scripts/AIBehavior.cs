using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIBehavior : MonoBehaviour
{
    public AIState aiState;
    public Transform playerPos;
    public Transform targetWaypoint;
    private NavMeshAgent agent;

    public List<Transform> wayPoints;
    public int waypointNumber;
    public bool isMoving;
    public float playerFollowRange;
    //public float followDuration;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        foreach (Transform t in targetWaypoint.GetComponentInChildren<Transform>())
        {
            wayPoints.Add(t.gameObject.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos.position);
        switch (aiState)
        {
            case AIState.FollowPlayer:
                agent.SetDestination(playerPos.position);
                //followDuration -= Time.deltaTime;
                break;
            case AIState.Patrol:
                //followDuration += Time.deltaTime;
                MultipleWaypointPatrol();
                break;
            default: break;

        }

        //if (distanceToPlayer <= playerFollowRange)
        //{
        //    aiState = AIState.FollowPlayer;
        //}
        //else if (distanceToPlayer != playerFollowRange)
        //{
        //    aiState = AIState.Patrol;

        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            aiState = AIState.FollowPlayer;
        }
    }

    void MultipleWaypointPatrol()
    {
        // returns true or false, if Ai has a next destination
        if (!agent.pathPending)
        {
            // returns the distance of the agent if it is the same of stopping distance
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // if agent has no path or agent is standing still
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    MoveToRandomWayPoint();
                }
            }
        }
    }

    void MoveToRandomWayPoint()
    {
        if (wayPoints.Count == 0)
        {
            Debug.LogWarning("No Waypoints");
            return;
        }

        isMoving = true;
        int newWaypointIndex = GetRandomWaypointIndex();

        // if waypoint number is not the same as waypoint index, 
        if (newWaypointIndex != waypointNumber)
        {
            // equal to random waypoint
            waypointNumber = newWaypointIndex;
            // setting the agent to new destination
            agent.SetDestination(wayPoints[waypointNumber].position);
        }
        else
        {
            MoveToRandomWayPoint();
        }
    }

    private int GetRandomWaypointIndex()
    {
        return Random.Range(0, wayPoints.Count);
    }

    private bool GetRandomBool()
    {
        return (true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerFollowRange);
    }

}

public enum AIState
{
    FollowPlayer,
    Patrol,
}
