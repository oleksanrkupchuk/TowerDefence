using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : MonoBehaviour, IPointerDownHandler {
    private AnimationEvent _openEvent = new AnimationEvent();
    private Animator _animator;
    private bool _isOpen = false;
    private Potion _potion;

    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    protected AnimationClip _openClip;
    [SerializeField]
    private ChestManager _chestManager;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void AddOpenEventForOpenAnimation() {
        float _playingAnimationTime = _openClip.length;
        _openEvent.time = _playingAnimationTime;
        _openEvent.functionName = nameof(EnablePotion);

        _openClip.AddEvent(_openEvent);
    }

    private void EnablePotion() {
        _chestManager.SpawnPotion(transform);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!_isOpen) {
            _isOpen = true;
            _animator.SetBool("isOpen", true);
        }
    }
}
