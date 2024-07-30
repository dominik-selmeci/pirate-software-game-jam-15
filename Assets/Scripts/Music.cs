using UnityEngine;
using UnityEngine.Audio;

public class Music : MonoBehaviour
{
    [SerializeField] AudioResource _darkTheme;
    [SerializeField] AudioResource _lightTheme;
    [SerializeField] AudioResource _roomCleansed;

    AudioSource _audioSource;
    ProgressManager _progressManager;
    private bool _isPlayingOnce;
    private bool _isRoomCleansed = false;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _progressManager = GetComponentInChildren<ProgressManager>();
    }

    void Start()
    {
        LoopPlay(_darkTheme);
    }

    void Update()
    {
        if (_progressManager.isRoomCleansed && !_isRoomCleansed)
        {
            _isRoomCleansed = true;
            PlayOnce(_roomCleansed);
        }

        if (_isPlayingOnce && !_audioSource.isPlaying)
        {
            LoopPlay(_lightTheme);
            _isPlayingOnce = false;
        }
    }

    void PlayOnce(AudioResource source)
    {
        _audioSource.loop = false;
        _audioSource.resource = source;
        _audioSource.Play();
        _isPlayingOnce = true;
    }

    void LoopPlay(AudioResource source)
    {
        _audioSource.loop = true;
        _audioSource.resource = source;
        _audioSource.Play();
    }
}
