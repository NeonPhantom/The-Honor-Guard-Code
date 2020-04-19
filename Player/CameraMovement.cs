using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject followTarget;
    public float moveSpeed;
    public bool cutsceneRotation = false;
    private Quaternion rotate;

    void Start () {
		
	}
	
	void Update () {
        if (!cutsceneRotation)
        {
            //var rotate = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            rotate = followTarget.transform.rotation;
        } else
        {
            rotate = new Quaternion(0, 0, 0, 0);
        }

        if (followTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, followTarget.transform.position, Time.deltaTime * moveSpeed);
            if (!cutsceneRotation)
            {
                transform.rotation = rotate;
            }
        }
	}
}
