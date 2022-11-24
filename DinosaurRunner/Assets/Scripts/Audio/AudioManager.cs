using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _additionalAudioSource;

    [Header("Main audio clips")]
    [SerializeField] private AudioClip _mainMenuAudio;
    [SerializeField] private AudioClip _gameAudio;

    [Header("Additional audio clips")]
    [SerializeField] private AudioClip _pressButtonAudio;
    [SerializeField] private AudioClip _jumpAudio;
    [SerializeField] private AudioClip _crouchAudio;
    [SerializeField] private AudioClip _diePlayerAudio;

    private AudioSource _audioSource;

    public void Initialize(float musicVolume, float eventVolume)
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = musicVolume;
        _additionalAudioSource.volume = eventVolume;
    }

    public void PlayMainMenuAudio() => PlayAudioClip(_mainMenuAudio);
    public void PlayGameAudio() => PlayAudioClip(_gameAudio);
    public void PlayPressButtonAudio() => PlayAdditionalAudioClip(_pressButtonAudio);
    public void PlayJumpAudio() => PlayAdditionalAudioClip(_jumpAudio);
    public void PlayCrouchAudio() => PlayAdditionalAudioClip(_crouchAudio);
    public void PlayDiePlayerAudio() => PlayAdditionalAudioClip(_diePlayerAudio);

    private void PlayAudioClip(AudioClip audio)
    {
        _audioSource.clip = audio;
        _audioSource.Play();
    }

    private void PlayAdditionalAudioClip(AudioClip audio)
    {
        _additionalAudioSource.clip = audio;
        _additionalAudioSource.Play();
    }
}