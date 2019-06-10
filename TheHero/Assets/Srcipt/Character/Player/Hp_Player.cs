using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp_Player : MonoBehaviour {
    public Image Hp_bar;
    public float Hp_start;
    public float Hp_current;

    private void Start()
    {
        Hp_current = Hp_start;
        update_HpBar();
    }

    public void getHeal(float healPoint)
    {
        Hp_current += healPoint;

        if(Hp_current > Hp_start)
        {
            Hp_current = Hp_start;
        }

        update_HpBar();
    }

    public void getDanmge(float dmg)
    {
        Hp_current -= dmg + (PlayerPrefs.GetInt("rock_lv", 0) * 0.1f);
        update_HpBar();

        //die
        if(Hp_current <= 0)
        {
            //die
        }
    }

    void update_HpBar()
    {
        Hp_bar.fillAmount = Hp_current / Hp_start;
    }
}
