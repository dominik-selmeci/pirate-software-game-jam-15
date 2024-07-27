using UnityEngine;

public class ProgressManager : MonoBehaviour
{

    [SerializeField] ProgressBar progressBar;
    int totalSpots;

	private void Awake()
	{
        totalSpots = FindObjectsOfType<OminousSpot>().Length;
        progressBar.SetMaxProgress(totalSpots);
	}

    // Update is called once per frame
    void Update()
    {
        int progress = (totalSpots - FindObjectsOfType<OminousSpot>().Length);
        progressBar.SetProgress(progress);
    }
}
