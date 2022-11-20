using UnityEngine;

public class SoundDisabled : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    void Update()
    {
        if (!_audioSource.isPlaying) {
            gameObject.SetActive(false);
        }
    }
}
