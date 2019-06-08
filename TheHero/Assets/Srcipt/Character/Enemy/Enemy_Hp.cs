using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hp : MonoBehaviour {
    public float hp_start;
    public float hp_current;
    public GameObject hp_txt_PopUp;
    public float txt_height;

    private void Start()
    {
        hp_current = hp_start;
    }

    public void tacked_Danmge(float dmg)
    {
        hp_current -= dmg;
        //pop up txt
        spawnTextPopUp((int)hp_current);

        if(hp_current <= 0)
        {
            //die
            
        }
    }

    void spawnTextPopUp(int txt)
    {
        GameObject go = Instantiate(hp_txt_PopUp, transform.position + (Vector3.up * txt_height), Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = txt.ToString();
    }
}
