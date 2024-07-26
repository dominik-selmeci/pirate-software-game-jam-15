using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
	private Queue<string> _sentences;
	public TextMeshProUGUI _dialogText;
	public TextMeshProUGUI _dialogButtonText;
	public Animator _dialogAnimator;

	void Start()
	{
		_sentences = new Queue<string>();
	}

	public void StartDialog(Dialog dialog)
	{
		_sentences.Clear();

		foreach (string sentence in dialog._sentences)
		{
			_sentences.Enqueue(sentence);
		}
		_dialogAnimator.SetBool("isOpen", true);

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if(_sentences.Count > 1)
		{
			_dialogButtonText.text = "Next";
		}else
		{
			_dialogButtonText.text = "Exit";
		}

		if (_sentences.Count == 0)
		{
			EndDialog();
			return;
		}

		string sentence = _sentences.Dequeue();
		_dialogText.text = sentence;
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	public void EndDialog()
	{
		_dialogAnimator.SetBool("isOpen", false);
	}

	IEnumerator TypeSentence(string sentence)
	{
		_dialogText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			_dialogText.text += letter;
			yield return null;
		}
	}
}
