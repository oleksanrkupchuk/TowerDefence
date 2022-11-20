using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTesting : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> _audios = new List<AudioSource>();
    [SerializeField]
    private float _delay;

    public void PlayAudios() {
        StartCoroutine(PlayAudiosInList());
    }

    private IEnumerator PlayAudiosInList() {
        foreach (var _audio in _audios) {
            yield return new WaitForSeconds(_delay);
            _audio.Play();
        }
    }
}
