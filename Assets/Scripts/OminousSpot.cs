using UnityEngine;

public class OminousSpot : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider);
        if (collider.GetComponent<TorchItem>() != null)
            Destroy(gameObject);
    }
}
