using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmBuyAbilityWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    public Button yes;
    public Button no;

    public void SetDescription(int price) {
        description.text = ShopMenuText.GetDescriptionForModalWindow(price);
    }

    public void Disable() {
        gameObject.SetActive(false);
    }

    public void Enable() {
        gameObject.SetActive(true);
    }
}
