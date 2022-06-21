using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour {
    private List<Potion> _potions = new List<Potion>();
    private bool _isSpawnPotion = false;
    private int _countChest;

    [SerializeField]
    private int _amountChestInMap;
    [SerializeField]
    private int _chanseSpawnPotion;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Chest[] _chests;
    [SerializeField]
    private List<Potion> _potionsPrefabs = new List<Potion>();

    private void Awake() {
        SpawnPotions();
        foreach (Chest chest in _chests) {
            chest.AddOpenEventForOpenAnimation();
        }
    }

    private void SpawnPotions() {
        for (int i = 0; i < _potionsPrefabs.Count; i++) {
            Potion _potion = Instantiate(_potionsPrefabs[i], transform.position, Quaternion.identity);
            _potion.transform.SetParent(_canvas.transform);
            _potion.transform.localScale = new Vector3(1f, 1f, 1f);
            _potion.gameObject.SetActive(false);
            _potions.Add(_potion);
        }
    }

    public void SpawnPotion(Transform chest) {
        _countChest++;

        if (!_isSpawnPotion) {
            int _chace = Random.Range(0, 100);
            if (_chace <= _chanseSpawnPotion) {
                SpawnPotionOnChest(chest);
            }

            if (_countChest >= _amountChestInMap) {
                SpawnPotionOnChest(chest);
            }
        }
    }

    private void SpawnPotionOnChest(Transform newPosition) {
        Potion _potion = GetRandomPotion();
        _potion.transform.position = newPosition.position;
        _potion.Init();
        _potion.gameObject.SetActive(true);
        _isSpawnPotion = true;
    }

    private Potion GetRandomPotion() {
        int _random = Random.Range(0, _potions.Count);
        return _potions[_random];
    }
}
