using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;
    [Range(0, 100)] public float speed;
    [Range(1, 500)] public float walkRadius;
    AIController controller;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<AIController>();
        if (agent != null)
        {
            agent.speed = speed;
            agent.SetDestination(RandomNavMeshLocation());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<OVRGrabbable>().isGrabbed)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            controller.enabled = false;
        }
        else if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(RandomNavMeshLocation());
        }
    }

    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPos = Vector3.zero;
        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomPos += transform.position;
        if(NavMesh.SamplePosition(randomPos, out NavMeshHit hit, walkRadius, 1))
        {
            finalPos = hit.position;
        }
        return finalPos;
    }
}
