using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponCollider : MonoBehaviour {
    public float dmg;
    public string target_tag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == target_tag)
        {
            if(target_tag == "Player")
            {
                other.GetComponent<Hp_Player>().getDanmge(dmg);
            }
            else if(target_tag == "Enemy")
            {
                other.GetComponent<Enemy_Hp>().tacked_Danmge(dmg);
            }
        }
    }

}
