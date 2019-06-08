using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDmg : MonoBehaviour {
    public float destrosy_time;
    public Vector3 randomPos = new Vector3(0.01f, 0.01f, 0.01f);

	// Use this for initialization
	void Start ()
    {
        transform.localPosition += new Vector3
            (
            Random.Range(-randomPos.x, randomPos.x),
            0,
            Random.Range(-randomPos.z, randomPos.z)
            );

        Destroy(this.gameObject, destrosy_time);
	}
}
