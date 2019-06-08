using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hp : MonoBehaviour {
    public float hp_start;
    public float hp_current;
    public TextMesh hp_txt_PopUp;

    private void Start()
    {
        hp_current = hp_start;
    }

    public void tacked_Danmge(float dmg)
    {
        hp_current -= dmg;
        //pop up txt

        if(hp_current <= 0)
        {
            //die
            
        }
    }

}
