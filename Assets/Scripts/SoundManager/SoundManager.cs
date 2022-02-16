using System;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField]
    private Sound[] _sounds;

    public static SoundManager Instance;

    private void OnEnable() {
        Init();
        AddAudioSourceForSounds();
    }

    private void Init() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void AddAudioSourceForSounds() {
        foreach (var sound in _sounds) {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start() {
        PlaySound("Background");
    }

    public void PlaySound(string soundName) {
        Sound _sound = Array.Find(_sounds, _sound => _sound.name == soundName);
        if(_sound == null) {
            print($"<color=red> Sound {soundName} is null </color>");
            return;
        }

        _sound.audioSource.loop = _sound.loop;
        _sound.audioSource.clip = _sound.audioClip;
        _sound.audioSource.volume = _sound.volume;
        _sound.audioSource.pitch = _sound.pitch;
        _sound.audioSource.spatialBlend = _sound.spaceSound;

        _sound.audioSource.Play();
    }

    public void SetSoudnsVolume(float value) {
        foreach (var sound in _sounds) {
            sound.volume = value;
            sound.audioSource.volume = value;
        }
    }
}

[Serializable]
public class Sound {
    public string name;
    public bool loop;
    [Range(0f, 1f)]
    public float volume;
    [Range(-3f, 3f)]
    public float pitch;
    [Range(0f, 1f)]
    public float spaceSound;
    public AudioClip audioClip;
    [HideInInspector]
    public AudioSource audioSource;
}
