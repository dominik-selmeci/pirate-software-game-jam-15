using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
	[SerializeField] Dialog startLevelDialog;

	private Queue<string> _sentences;
	public TextMeshProUGUI _dialogText;
	public TextMeshProUGUI _dialogButtonText;
	public Animator _dialogAnimator;

	public bool isOpen;

	void Start()
	{
		if(_dialogAnimator != null)
		{
			_sentences = new Queue<string>();

			if (startLevelDialog?._sentences?.Length > 0)
			{
				StartDialog(startLevelDialog);
			}
		}
	}

	public void StartDialog(Dialog dialog)
	{
		_sentences.Clear();

		foreach (string sentence in dialog._sentences)
		{
			_sentences.Enqueue(sentence);
		}
		isOpen = true;
		_dialogAnimator.SetBool("isOpen", isOpen);

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (_sentences.Count > 1)
		{
			_dialogButtonText.text = "> Next <";
		}
		else
		{
			_dialogButtonText.text = "> Exit <";
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
		isOpen = false;
		_dialogAnimator.SetBool("isOpen", isOpen);
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
