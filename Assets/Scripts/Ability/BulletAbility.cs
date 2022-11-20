using UnityEngine;

public class BulletAbility : MonoBehaviour {

    [SerializeField]
    protected float _time;
    [SerializeField]
    private AudioSource _sound;

    public AudioSource Sound { get => _sound; }
}
