using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class commend : MonoBehaviour {
    public InputField m_input;

    //condition
    public bool addCoin = false;

	// Use this for initialization
	void Start ()
    {
		m_input.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            //start com
            m_input.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        //start com
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("Enter");
            if (m_input.text == "add_coin" && m_input.gameObject.active)
            {
                addCoin = true;
                m_input.text = "";
            }
            else if (addCoin && m_input.gameObject.active)
            {
                addCoin = false;
            }
        }

        //endter com var
    }

    public void closeInsertText()
    {
        m_input.gameObject.SetActive(false);
    }
}
