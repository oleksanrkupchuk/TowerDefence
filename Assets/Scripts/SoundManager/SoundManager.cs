using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public enum Sound {
    Background
}

public class SoundManager : MonoBehaviour {
    [SerializeField]
    private AudioSource _backgroundSound;

    [SerializeField]
    private AudioClip _clip;

    public static SoundManager Instance;

    private void OnEnable() {
        InitSoundManager();
    }

    private void Start() {
        PlaySound();
    }

    private void InitSoundManager() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound() {
        _backgroundSound.Play();
    }

    public void SetVolume(float value) {
        _backgroundSound.volume = value;
    }
}
