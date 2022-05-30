using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMinotaurDialog : MonoBehaviour
{
    [SerializeField]
    private NPCMinotaur _npcMinotaur;
    [SerializeField]
    private Button _accept;
    [SerializeField]
    private Button _declined;
    [SerializeField]
    private Button _ok;
    [SerializeField]
    private GameObject _choiseDialog;
    [SerializeField]
    private GameObject _thankDialog;

    private void Awake() {
        _choiseDialog.SetActive(false);
        _thankDialog.SetActive(false);

        _accept.onClick.AddListener(() => {
            _npcMinotaur.move = true;
            gameObject.SetActive(false);
        });
        _declined.onClick.AddListener(() => {
            _thankDialog.SetActive(true);
        });
        _ok.onClick.AddListener(() => {
            _npcMinotaur.SpawnGold();
            _npcMinotaur.gameObject.SetActive(false);
            gameObject.SetActive(false);
        });
    }

    public void ShowChoiseDialog() {
        _choiseDialog.SetActive(true);
    }
}
