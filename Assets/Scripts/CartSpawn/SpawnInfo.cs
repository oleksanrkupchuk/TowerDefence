using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnInfo : MonoBehaviour {
    private List<CartSpawnInfo> _carts = new List<CartSpawnInfo>();

    [SerializeField]
    private Transform _content;
    [SerializeField]
    private CartSpawnInfo _cartSpawnInfo;
    [SerializeField]
    private RectTransform _rect;
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private Scrollbar _scroll;
    [SerializeField]
    private VerticalLayoutGroup _verticalLayoutGroup;

    public bool isAccessSpawnInfo;
    [HideInInspector]
    public NewWaveIcon newWaveIcon;

    public void Awake() {
        SpawnCartInfo();

        _rect.anchoredPosition = new Vector2(185f, -167.5f);
        Disable();
    }

    private void SpawnCartInfo() {
        for (int numberCart = 0; numberCart < 50; numberCart++) {
            CartSpawnInfo _cartSpawnInfoObject = Instantiate(_cartSpawnInfo);
            _cartSpawnInfoObject.transform.SetParent(_content);
            _cartSpawnInfoObject.transform.localScale = new Vector3(1f, 1f);
            _cartSpawnInfoObject.gameObject.SetActive(false);
            _carts.Add(_cartSpawnInfoObject);
        }
    }

    public void EnableCartInfo(List<EnemySpawnRules> rules, NewWaveIcon newWave) {
        newWaveIcon = newWave;
        DisableCartInfo();
        SetSizeObject(rules.Count);

        for (int numberRules = 0; numberRules < rules.Count; numberRules++) {
            _carts[numberRules].Init(rules[numberRules].enemy.CartSpawnInfoData, rules[numberRules].amount);
            _carts[numberRules].gameObject.SetActive(true);
        }

        gameObject.SetActive(true);
    }

    private void SetSizeObject(int amountRules) {
        if(amountRules == 1) {
            CalculateHeight(amountRules);
            _rect.anchoredPosition = new Vector2(185f, -65f);
            HideScroll();
        }
        else if(amountRules == 2) {
            CalculateHeight(amountRules);
            _rect.anchoredPosition = new Vector2(185f, -117.5f);
            HideScroll();
        }
        else if (amountRules == 3) {
            CalculateHeight(amountRules);
            _rect.anchoredPosition = new Vector2(185f, -167.5f);
            HideScroll();
        }
        else {
            DefaultSize();
            _rect.anchoredPosition = new Vector2(185f, -167.5f);
            EnableScroll();
        }
    }

    private void CalculateHeight(int amountRules) {
        float _height = (amountRules * _cartSpawnInfo.RectTransform.rect.height) + _verticalLayoutGroup.padding.top +
            _verticalLayoutGroup.padding.bottom + ((amountRules - 1) * _verticalLayoutGroup.spacing);
        _rect.sizeDelta = new Vector2(350f, _height);
    }

    private void DefaultSize() {
        _rect.sizeDelta = new Vector2(350f, 320f);
    }

    private void HideScroll() {
        _scrollRect.verticalScrollbar = null;
        _scroll.gameObject.SetActive(false);
    }

    private void EnableScroll() {
        _scrollRect.verticalScrollbar = _scroll;
        _scroll.gameObject.SetActive(true);
    }

    private void DisableCartInfo() {
        foreach (CartSpawnInfo cart in _carts) {
            cart.gameObject.SetActive(false);
        }
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}
