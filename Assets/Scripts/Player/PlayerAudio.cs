using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip _drinkPotionSFX;
    [SerializeField] AudioClip _placeItemSFX;
    [SerializeField] AudioClip _collectMaterialSFX;
    AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayCollectSound() => _audioSource.PlayOneShot(_collectMaterialSFX);
    public void PlayPlaceItemSound() => _audioSource.PlayOneShot(_placeItemSFX);
    public void PlayDrinkSound() => _audioSource.PlayOneShot(_drinkPotionSFX);
}
