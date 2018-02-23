using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour {
    public float ScrollSpeed;
    public float ScrollAreaSize;
    Transform cameraTransform;
	// Use this for initialization
	void Start () {
        cameraTransform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        Vector3 scrollVector = new Vector3();
        if (mouseX < Screen.width * ScrollAreaSize)
        {
            scrollVector += Vector3.left;
        } else if (mouseX > Screen.width * (1 - ScrollAreaSize))
        {
            scrollVector += Vector3.right;
        }

        if (mouseY < Screen.height * ScrollAreaSize) 
        {
            scrollVector += Vector3.down;
        } else if (mouseY > Screen.height * (1 - ScrollAreaSize))
        {
            scrollVector += Vector3.up;
        }

        cameraTransform.Translate(scrollVector * ScrollSpeed * Time.deltaTime);

	}

}
