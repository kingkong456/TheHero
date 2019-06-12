using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour {
    public Animator BlackPart;
    public Dialogue dialogue;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player");
            BlackPart.SetTrigger("cutScene");
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            if (FindObjectOfType<PlayerControllerNUmber2>() != null)
            {
                FindObjectOfType<PlayerControllerNUmber2>().m_state = player_state.on_ui;
            }
        }
    }

}
