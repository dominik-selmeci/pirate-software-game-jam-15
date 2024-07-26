using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
	private Queue<string> sentences;
	public TextMeshProUGUI dialogText;
	public Animator dialogAnimator;

	void Start()
	{
		sentences = new Queue<string>();
	}

	public void StartDialog(Dialog dialog)
	{
		sentences.Clear();

		foreach (string sentence in dialog._sentences)
		{
			sentences.Enqueue(sentence);
		}
		dialogAnimator.SetBool("isOpen", true);

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialog();
			return;
		}

		string sentence = sentences.Dequeue();
		dialogText.text = sentence;
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	public void EndDialog()
	{
		dialogAnimator.SetBool("isOpen", false);
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogText.text += letter;
			yield return null;
		}
	}
}
