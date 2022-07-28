using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetAvoidBeingStuck : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if(navMeshAgent == null)
        {
            Debug.Log("cannot find nav mesh agent in " + name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!navMeshAgent.hasPath && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        //{
        //    Debug.Log("Character stuck");
        //    navMeshAgent.enabled = false;
        //    navMeshAgent.enabled = true;
        //    Debug.Log("navmesh re enabled");
        //    // navmesh agent will start moving again
        //}
    }
}
