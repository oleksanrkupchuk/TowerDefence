using UnityEngine;
using UnityEngine.UI;

public class ConfirmBuyAbilityWindow : MonoBehaviour
{
    public Button yes;
    public Button no;

    public void Disable() {
        gameObject.SetActive(false);
    }

    public void Enable() {
        gameObject.SetActive(true);
    }
}
