using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour {
    public Transform target;
    public float smooth_time;
    public float cameraHight;

	// Update is called once per frame
	void Update ()
    {
        if(target == null)
        {
            target = this.transform;
        }

        Vector3 NewPos = new Vector3(target.position.x, target.position.y + cameraHight, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, NewPos, smooth_time * Time.deltaTime);
	}
}
