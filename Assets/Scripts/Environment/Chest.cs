using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class Chest : MonoBehaviour {
    private AnimationEvent _openEvent = new AnimationEvent();
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private bool _isOpen = false;
    private Potion _potion;

    [SerializeField]
    protected AnimationClip _openClip;
    [SerializeField]
    private ChestManager _chestManager;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
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

    private void OnMouseDown() {
        if (!_isOpen) {
            _isOpen = true;
            _animator.SetBool("isOpen", true);
        }
    }
}
