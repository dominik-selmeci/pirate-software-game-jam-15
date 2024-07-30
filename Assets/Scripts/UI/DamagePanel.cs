using UnityEngine;

public class DamagePanel : MonoBehaviour
{
	public void EnablePanel()
	{
		gameObject.SetActive(true);
	}

	public void DisabelPanel()
	{
		gameObject.SetActive(false);
	}
}
