using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(NavMeshAgent))]

public class AIController : MonoBehaviour
{
    public GameObject scoreManager;
    public NavMeshAgent agent;
    [Range(0, 100)] public float speed;
    [Range(1, 500)] public float walkRadius;
    AIController controller;
    private bool hasBeenGrabbed = false;
    private Vector3 targetPosition;
    private AIState currentState;
    public float fleeDistance = 5f;
    public Transform player;
    private enum AIState
    {
        Roaming,
        Fleeing
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        currentState = AIState.Roaming;
        targetPosition = RandomNavMeshLocation();
    }

    // Update is called once per frame
    void Update()
    {
        //checks to see if AI was grabbed and if it was grabbed its AI and NavMesh get turned off
        //else the AI contiunes to randomly move in the area set for it
        if (transform.GetComponent<OVRGrabbable>().isGrabbed)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            controller.enabled = false;
            if (hasBeenGrabbed == false)
            {
                hasBeenGrabbed = true;
                scoreManager.GetComponent<ScoreController>().score++;
            }
        }
        switch (currentState)
        {
            case AIState.Roaming:
                Roam();
                break;

            case AIState.Fleeing:
                Flee();
                break;
        }
    }
    //logic used to find and set to randonly move in the NavMesh and return the position
    private Vector3 RandomNavMeshLocation()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection.y = 0;

        Vector3 randomPos = transform.position + randomDirection;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPos, out hit, walkRadius, 1))
        {
            return hit.position;
        }

        return transform.position;
    }
    private void Roam()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            targetPosition = RandomNavMeshLocation();
            agent.SetDestination(targetPosition);
        }
    }
    private void Flee()
    {
        Vector3 fleeDirection = transform.position - player.position;
        fleeDirection.y = 0;

        if (fleeDirection.magnitude > fleeDistance)
        {
            currentState = AIState.Roaming;
            targetPosition = RandomNavMeshLocation();
            agent.SetDestination(targetPosition);
        }
        else
        {
            Vector3 fleeDestination = transform.position + fleeDirection.normalized * fleeDistance;
            agent.SetDestination(fleeDestination);
        }
    }
}
