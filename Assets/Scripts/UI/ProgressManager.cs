using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    int _progress;

    [SerializeField] ProgressBar progressBar;
    int totalSpots;
    Enemy[] enemies;

    [HideInInspector] public bool isRoomCleansed = false;

    private void Awake()
    {
        totalSpots = FindObjectsOfType<OminousSpot>().Length;
        progressBar.SetMaxProgress(totalSpots);
        enemies = FindObjectsOfType<Enemy>();
        progressBar.SetProgress(0);
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

        if (newProgress == totalSpots && !isRoomCleansed)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.Die();
                }
            }

            isRoomCleansed = true;
        }
    }
}
