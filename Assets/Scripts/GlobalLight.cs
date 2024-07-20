using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLight : MonoBehaviour
{
    void Start()
    {
        GetComponent<Light2D>().intensity = 0.01f;
    }
}
