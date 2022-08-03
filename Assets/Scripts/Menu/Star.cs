using System.Collections;
using UnityEngine;

public class Star : MonoBehaviour {
    private Vector3 _toScale;

    private void OnEnable() {
        _toScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public IEnumerator IncreaseObject() {
        LeanTween.scale(gameObject, _toScale, 0.8f).setEaseOutElastic();
        yield return new WaitForSeconds(1);
    }
}
