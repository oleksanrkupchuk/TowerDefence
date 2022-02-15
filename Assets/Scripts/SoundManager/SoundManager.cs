using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField]
    private Sound[] _sounds;
    //private List<Sound> _sounds = new List<Sound>();

    public static SoundManager Instance;

    private void OnEnable() {
        Init();
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

    private void Start() {
        PlaySound("Background", null);
    }

    public void PlaySound(string soundName, AudioSource audioSource) {
        //Sound _sound = _sounds.FirstOrDefault(sound => sound.name == soundName);
        Sound _sound = Array.Find(_sounds, _sound => _sound.name == soundName);
        if(_sound == null) {
            print($"<color=red> Sound {soundName} is null </color>");
            return;
        }

        bool _remove = false;
        if(audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
            _remove = true;
        }

        audioSource.loop = _sound.loop;
        audioSource.clip = _sound.audioClip;
        audioSource.volume = _sound.volume;
        audioSource.pitch = _sound.pitch;
        audioSource.spatialBlend = _sound.spaceSound;
        audioSource.Play();

        if(!_sound.loop && _remove) {
            Destroy(audioSource, _sound.audioClip.length);
        }
    }

    public void SetVolume(float value) {
        //_backgroundSound.volume = value;
    }
}

[System.Serializable]
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
}
