using UnityEngine;

public class OminousBlockingSpot : MonoBehaviour
{
    void Update()
    {
        int ominousSpotCount = FindObjectsOfType<OminousSpot>().Length;
        if (ominousSpotCount == 0)
            Destroy(gameObject);
    }
}
