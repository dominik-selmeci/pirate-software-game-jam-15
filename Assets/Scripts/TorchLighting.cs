using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchLighting : MonoBehaviour
{
    [SerializeField] float _minIntensity = 0.5f;
    [SerializeField] float _maxIntensity = 1.7f;

    Light2D _light2D;

    void Awake()
    {
        _light2D = GetComponent<Light2D>();
    }

    void Start()
    {
        InvokeRepeating(nameof(Flicker), 0, 0.07f);
    }

    void Flicker()
    {
        _light2D.intensity = Random.Range(_minIntensity, _maxIntensity);
    }
}
