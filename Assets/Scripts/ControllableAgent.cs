using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class ControllableAgent : NetworkBehaviour
{
    [SyncVar]
    public GameObject TargetUnit;

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
        Debug.Log("Turning Agent.");
        gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, Time.deltaTime * agent.angularSpeed);
    }

    public void SetAttackTarget(GameObject newTarget)
    {
        TargetUnit = newTarget;
        shooting.SpecifiedTarget = newTarget;
    }

    public void SetNavTarget(Vector3 newTarget, Quaternion newOrientation)
    {
        targetPosition = newTarget;
        targetOrientation = newOrientation;
        shooting.SpecifiedTarget = null;
        Debug.Log("Target Orientation: " + newOrientation.ToString());
    }

}
