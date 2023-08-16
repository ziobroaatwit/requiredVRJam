using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.VFX;

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
    private State currentState = State.Roam;
    public float fleeDistance = 5f;
    public Transform player;
    [SerializeField] VisualEffect POOF;
    public Boolean isStopped = false;
    private enum State
    {
        Roam,
        Flee
    }

    private Transform target; 
    private NavMeshAgent navMeshAgent;
    public float roamRadius = 5f;
    public float fleeRadius = 3f;

    private void Start()
    {
        currentState = State.Roam;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (transform.GetComponent<OVRGrabbable>().isGrabbed)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            if (hasBeenGrabbed == false)
            {
                controller.enabled = false;
            }
        }
        if(Math.Abs(this.transform.rotation.eulerAngles.z) >= 50 || Math.Abs(this.transform.rotation.eulerAngles.x) >= 50)
        {
            hasBeenGrabbed = true;
            scoreManager.GetComponent<ScoreController>().score++;
            GameObject VE = Instantiate(POOF.gameObject, this.transform.position, Quaternion.identity);
            VE.GetComponent<VisualEffect>().Play();
            Destroy(this.gameObject);
        }
        switch (currentState)
        {
            case State.Roam:
                Roam();
                break;
            case State.Flee:
                Flee();
                break;
        }
    }

    private void Roam()
    {
        if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.5f)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * roamRadius;
            randomDirection += transform.position;

            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(randomDirection, out navMeshHit, roamRadius, NavMesh.AllAreas);

            Vector3 destination = navMeshHit.position;
            navMeshAgent.SetDestination(destination);
        }

        if (Vector3.Distance(transform.position, target.position) <= fleeDistance)
        {
            currentState = State.Flee;
        }
    }

    private void Flee()
    {
        Vector3 fleeDirection = (transform.position - target.position).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * fleeRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(fleePosition, out hit, fleeRadius, NavMesh.AllAreas);
        Vector3 destination = hit.position;
        navMeshAgent.SetDestination(destination);
        if (Vector3.Distance(transform.position, target.position) > fleeRadius)
        {
            currentState = State.Roam;
        }
    }
}
