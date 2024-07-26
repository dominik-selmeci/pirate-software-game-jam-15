using UnityEngine;
using TMPro;
using System.Collections;

public class DialogController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _dialogText;
    private int _index = 0;
    private bool _startDialog = true;
    private bool _isWriting = false;
  
    public string[] _sentences;
    public float _dialogSpeed;
    public Animator _dialogAnimator;

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			if (_startDialog)
			{
				_dialogAnimator.SetTrigger("Enter");
				_startDialog = false;
				_dialogText.text = "";
				NextSentence();
			}else
			{
				if(_dialogText.text != "")
				{
					_dialogText.text = _sentences[_index];
				}
			
				NextSentence();
			}
		}
	}

	void NextSentence()
	{

		if (_index <= _sentences.Length - 1)
		{
			_dialogText.text = "";
			StartCoroutine(WriteSentence());
		}
		else
		{
			_dialogText.text = "";
			_dialogAnimator.SetTrigger("Exit");
			_index = 0;
			_startDialog = true;
		}
	}

	IEnumerator WriteSentence()
	{
		_isWriting = true;
        foreach (char Character in _sentences[_index].ToCharArray())
		{
            _dialogText.text += Character;
            yield return new WaitForSeconds(_dialogSpeed);
		}

        _index++;
		_isWriting = false;
    }
}
