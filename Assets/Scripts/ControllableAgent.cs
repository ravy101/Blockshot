using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class ControllableAgent : NetworkBehaviour
{
    //[SyncVar]
    private GameObject TargetUnit;

    ShootingBehaviour shooting;
    NavMeshAgent agent;
    Shader initialShader;
    [SyncVar]
    Vector3 targetPosition;
    [SyncVar]
    Quaternion targetOrientation;

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        initialShader = gameObject.GetComponent<Renderer>().material.shader;
        shooting = gameObject.GetComponent<ShootingBehaviour>();
        targetPosition = gameObject.transform.position;
        targetOrientation = gameObject.transform.rotation;

    }

    // Update is called once per frame
    void Update() {
        agent.SetDestination(targetPosition);
        if (agent.velocity == Vector3.zero && gameObject.transform.rotation != targetOrientation)
        {
            TurnTowardTarget();
        }
        if (TargetUnit != null)
        {
            TryAttackTarget(TargetUnit);
        }
    }

    public void SetHighlight(bool highlight)
    {
        if (highlight)
        {
            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Silhouette");
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.shader = initialShader;
        }
    }

    private void TurnTowardTarget()
    {
        gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, Time.deltaTime * agent.angularSpeed);
    }

    public void TryAttackTarget(GameObject target)
    {
		
        TargetUnit = target;
        //try to stop and shoot
		if (shooting.CanHit (target, true)) {
			targetPosition = gameObject.transform.position;
			targetOrientation = Quaternion.LookRotation (target.transform.position - gameObject.transform.position);
			shooting.SpecifiedTarget = target;
		} else {//otherwise move toward target
			targetPosition = target.transform.position;
		}
        
    }

    public void SetNavTarget(Vector3 newTarget, Quaternion newOrientation)
    {
       
        targetPosition = newTarget;
        targetOrientation = newOrientation;
        TargetUnit = null;
        shooting.SpecifiedTarget = null;
       
    }
    
}
