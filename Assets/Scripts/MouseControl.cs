using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MouseControl : NetworkBehaviour {

    public GameObject Selection;
    public GameObject DragIndicator;

    ControllableAgent agent;
    Camera playerCamera;
    Vector3 targetDestination;
    Quaternion targetOrientation;
    bool settingOrientation;

    private Vector3 DragIndicatorOffset = new Vector3(0, 1, 0); //offset to bring the drag indicator up off the terrain

	// Use this for initialization
	void Start () {
        playerCamera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) // Select Click
        {
            Ray clickRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(clickRay, out rayHit))
            {
                if (Selection != null)
                {
                    //deselect stuff
                    DeselectObject(Selection);
                }

                if (rayHit.collider.gameObject.tag == "Selectable")
                {
                    SelectObject(rayHit.collider.gameObject);
                }
                
            }
        }
        else if (!settingOrientation && Input.GetMouseButtonDown(1) && Selection != null) /*Begin command click. TODO add limitations for whether player can control the selected unit*/
        {
            Ray clickRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(clickRay, out rayHit))
            {
                if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Unit"))
                {
                    //right click on a unit
                    agent.SetAttackTarget(rayHit.collider.gameObject);
                }
                else { 
                    targetDestination = rayHit.point;
                    settingOrientation = true;
                }
            }
        }else if (settingOrientation && Input.GetMouseButton(1) && Selection != null) // Update command click-drag
        {
            Ray dragRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit dragHit;
            if (Physics.Raycast(dragRay, out dragHit))
            {
                SetDragIndicator(targetDestination, dragHit.point);
            }
        }
        else if (settingOrientation && Input.GetMouseButtonUp(1) && Selection != null) //End command click.
        {
            Ray releaseRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit releaseHit;
            if (Physics.Raycast(releaseRay, out releaseHit))
            {
                targetOrientation = Quaternion.LookRotation(releaseHit.point - targetDestination);
            }
            DragIndicator.GetComponent<MeshRenderer>().enabled = false;
            agent.SetNavTarget(targetDestination, targetOrientation);
            settingOrientation = false;
        }

    }

    private void SelectObject(GameObject gameObject)
    {
        //do stuff to gameObject
        Selection = gameObject;
        agent = gameObject.GetComponent<ControllableAgent>();
        agent.SetHighlight(true);

    }

    private void DeselectObject(GameObject gameObject)
    {
        //undo stuff to gameObject
        agent.SetHighlight(false);
        Selection = null;
        agent = null;
    }

    private void SetDragIndicator(Vector3 from, Vector3 to)
    {
        if (from != to)
        {
            DragIndicator.GetComponent<MeshRenderer>().enabled = true;
            from += DragIndicatorOffset;
            to += DragIndicatorOffset;
            Vector3 v3 = to - from;
            DragIndicator.transform.position = from + (v3) / 2.0f;
            DragIndicator.transform.localScale = new Vector3(0.5f, v3.magnitude/2, 0.5f);
            DragIndicator.transform.rotation = Quaternion.FromToRotation(Vector3.up, v3);
        }
    }

}
