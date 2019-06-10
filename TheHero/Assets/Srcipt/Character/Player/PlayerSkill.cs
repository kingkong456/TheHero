using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour {

    [Header("UI")]
    public GameObject skill_Nevergation;
    public GameObject[] fire_lv_gate;
    public GameObject[] water_lv_gate;
    public GameObject[] rock_lv_gate;

    public static PlayerSkill instace;

    private void Awake()
    {
        instace = this;
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();

        update_skillGate();
        close_skill_navergation();
    }

    public void fire_add(int point_add)
    {
        if (PlayerPrefs.GetInt("coin", 0) >= PlayerPrefs.GetInt("fire_lv", 0) + 1)
        {
            Debug.Log(PlayerPrefs.GetInt("fire_lv", 0));
            fire_lv_gate[PlayerPrefs.GetInt("fire_lv", 0)].SetActive(true);
            PlayerCoin.instane.de_Coin(PlayerPrefs.GetInt("fire_lv", 0) + 1);
            PlayerPrefs.SetInt("fire_lv", PlayerPrefs.GetInt("fire_lv", 0) + point_add);
        }
    }

    public void water_add(int point_add)
    {
        if (PlayerPrefs.GetInt("coin", 0) >= PlayerPrefs.GetInt("water_lv", 0) + 1)
        {
            water_lv_gate[PlayerPrefs.GetInt("water_lv", 0)].SetActive(true);
            PlayerCoin.instane.de_Coin(PlayerPrefs.GetInt("fire_lv", 0) + 1);
            PlayerPrefs.SetInt("water_lv", PlayerPrefs.GetInt("water_lv", 0) + point_add);
        }
    }

    public void rock_add(int point_add)
    {
        if (PlayerPrefs.GetInt("coin", 0) >= PlayerPrefs.GetInt("rock_lv", 0) + 1)
        {
            rock_lv_gate[PlayerPrefs.GetInt("rock_lv", 0)].SetActive(true);
            PlayerCoin.instane.de_Coin(PlayerPrefs.GetInt("fire_lv", 0) + 1);
            PlayerPrefs.SetInt("rock_lv", PlayerPrefs.GetInt("rock_lv", 0) + point_add);
        }
    }

    public void open_skill_navegation()
    {
        skill_Nevergation.SetActive(true);
    }

    public  void close_skill_navergation()
    {
        skill_Nevergation.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerNUmber2>().m_state = player_state.onGround;
    }

    public void update_skillGate()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("fire_lv", 0); i++)
        {
            fire_lv_gate[i].SetActive(true);
        }

        for (int i = 0; i < PlayerPrefs.GetInt("water_lv", 0); i++)
        {
            water_lv_gate[i].SetActive(true);
        }

        for (int i = 0; i < PlayerPrefs.GetInt("rock_lv", 0); i++)
        {
            rock_lv_gate[i].SetActive(true);
        }

    }

}
