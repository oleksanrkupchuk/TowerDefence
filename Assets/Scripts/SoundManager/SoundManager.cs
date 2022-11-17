using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private List<AudioSource> _externalSoundEffects = new List<AudioSource>();
    private SettingsData _settingData;

    [Header("Music")]
    [SerializeField]
    private Sound[] _sounds;

    [Header("Sound Effect")]
    [SerializeField]
    private Sound[] _soundsEffect;

    public static SoundManager Instance;

    private void OnEnable() {
        ClearExternalSoundEffect();
        Init();
        AddAudioSourceForSounds();
        AddAudioSourceForSoundsEffect();
    }

    private void ClearExternalSoundEffect() {
        for (int i = _externalSoundEffects.Count; i > 0; i--) {
            _externalSoundEffects.RemoveAt(i);
        }
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

    private void AddAudioSourceForSoundsEffect() {
        foreach (var sound in _soundsEffect) {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start() {
        _settingData = SaveSystemSettings.LoadSettings();
        InitSoundVolume(_settingData);
        InitEffectVolume(_settingData);
        //PlaySound(SoundName.Background);
    }

    public void InitSoundVolume(SettingsData settingData) {
        foreach (var sound in _sounds) {
            sound.volume = settingData.soundVolume;
            sound.audioSource.volume = settingData.soundVolume;
        }
    }

    public void InitEffectVolume(SettingsData settingData) {
        foreach (var effect in _soundsEffect) {
            effect.volume = settingData.effectVolume;
            effect.audioSource.volume = settingData.effectVolume;
        }
    }

    public void InitExternalEffectVolume(SettingsData settingData) {
        foreach (var externalEffect in _externalSoundEffects) {
            if (externalEffect.gameObject.activeSelf == true) {
                externalEffect.volume = settingData.effectVolume;
            }
        }
    }

    public void PlaySoundEffect(string soundName) {
        Play(soundName, _soundsEffect);
    }

    public void PlaySound(string soundName) {
        Play(soundName, _sounds);
    }

    private void Play(string soundName, Sound[] _sounds) {
        Sound _sound = Array.Find(_sounds, _sound => _sound.name == soundName);
        if (_sound == null) {
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

    public void SetSoundsVolume(float value) {
        foreach (var sound in _sounds) {
            sound.volume = value;
            sound.audioSource.volume = value;
        }
    }

    public void SetEffectsVolume(float value) {
        foreach (var effect in _soundsEffect) {
            effect.volume = value;
            effect.audioSource.volume = value;
        }
    }

    public void AddExternalEffect(AudioSource sound) {
        _externalSoundEffects.Add(sound);
    }

    public void RemoveExternalSound(AudioSource sound) {
        _externalSoundEffects.Remove(sound);
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

public static class SoundName {
    public static readonly string ButtonClick = "ButtonClick";
    public static readonly string Background = "Background";
    public static readonly string LoseGame = "LoseGame";
    public static readonly string WinGame = "WinGame";
    public static readonly string TowerUpgrade = "TowerUpgrade";
    public static readonly string ErrorSetTower = "ErrorSetTower";
    public static readonly string SellTower = "SellTower";
    public static readonly string HitEnemy = "HitEnemy";
    public static readonly string FireShot = "FireShot";
    public static readonly string IronShot = "IronShot";
    public static readonly string RockShot = "RockShot";
    public static readonly string StartWave = "StartWave";
    public static readonly string Explosion = "Explosion";
    public static readonly string PutTower = "PutTower";
    public static readonly string SelectTower = "SelectTower";
}
