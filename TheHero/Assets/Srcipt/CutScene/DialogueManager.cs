using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public Animator blackPart;

    [Header("Ui")]
	public Text dialogueText;
    public GameObject StoryPanel;

	private Queue<string> sentences;

	public void StartDialogue (Dialogue dialogue)
	{
        StoryPanel.SetActive(true);
        sentences = new Queue<string>();

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
        //end
        StoryPanel.SetActive(false);
        blackPart.SetTrigger("EndDi");
        FindObjectOfType<PlayerControllerNUmber2>().m_state = player_state.onGround;
    }

}
