using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    int _progress;

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
        int newProgress = (totalSpots - FindObjectsOfType<OminousSpot>().Length);
        if (newProgress != _progress)
        {
            _progress = newProgress;
            progressBar.SetProgress(_progress);
        }
    }
}
