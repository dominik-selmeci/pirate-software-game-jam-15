using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    int _progress;

    [SerializeField] ProgressBar progressBar;
    int totalSpots;
    Enemy[] enemies;

    private void Awake()
    {
        totalSpots = FindObjectsOfType<OminousSpot>().Length;
        progressBar.SetMaxProgress(totalSpots);
        enemies = FindObjectsOfType<Enemy>();
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

        Debug.Log("Total Spots " + totalSpots);
        Debug.Log("New Progress " + newProgress);

		if (newProgress == 1)
		{
			foreach (Enemy enemy in enemies)
			{
                if(enemy != null)
				{
                    enemy.Die();
                }
			}
		}
	}
}
