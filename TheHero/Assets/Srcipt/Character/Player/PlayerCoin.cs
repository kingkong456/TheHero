using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoin : MonoBehaviour {
    public Text coin_txt;

    public static PlayerCoin instane;

    private void Awake()
    {
        instane = this;
    }

    private void Start()
    {
        updateCoinTxt();
    }

    public void updateCoinTxt()
    {
        coin_txt.text = PlayerPrefs.GetInt("coin", 0).ToString();
    }

    public void addCoin(int coin_add)
    {
        PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin",0) + coin_add);
        updateCoinTxt();
    }

    public void de_Coin(int coin_de)
    {
        PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin", 0) - coin_de);
        updateCoinTxt();
    }
}
