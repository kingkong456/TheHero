using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour {
    public Animator BlackPart;
    public Dialogue dialogue;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            BlackPart.SetTrigger("cutScene");
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            FindObjectOfType<PlayerControllerNUmber2>().m_state = player_state.on_ui;
        }
    }

}
