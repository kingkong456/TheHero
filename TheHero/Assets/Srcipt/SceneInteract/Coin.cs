using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public int thisVar = 0;

    [SerializeField]
    GameObject destroy_particle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerCoin.instane.addCoin(thisVar);
            GameObject go = (GameObject)Instantiate(destroy_particle, this.transform.position + (Vector3.up * 0.55f), Quaternion.identity);
            Destroy(go, 2);
            Destroy(this.gameObject);
        }
    }

}
