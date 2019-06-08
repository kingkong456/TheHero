using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Seeker : MonoBehaviour {
    public Transform PlayerHadsee;

    [Header("Seeker ststus")]
    public float seeker_range;
    public float seeker_height;

    private void Update()
    {
        //checkPlayerInRange();
    }

    public void checkPlayerInRange()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position + (Vector3.up * seeker_height), Vector3.left, out hit, seeker_range))
        {
            if(hit.collider.tag == "Player")
            {
                PlayerHadsee = hit.collider.gameObject.transform;
            }
            else
            {
                PlayerHadsee = null;
            }
        }
        else
        {
            PlayerHadsee = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + (Vector3.up * seeker_height), (transform.position + (Vector3.up * seeker_height)) + (Vector3.left * seeker_range));
    }
}
