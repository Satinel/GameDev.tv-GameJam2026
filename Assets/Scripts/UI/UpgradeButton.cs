using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] Button _thisButton;
    [SerializeField] IntReferenceSO _upgrade, _score;
    [SerializeField] TextMeshProUGUI _buttonPriceText;
    // [SerializeField] FloatingText _floatingTextPrefab;

    void Start()
    {
        SetNewPrice();
    }

    public void Purchase()  // UI Button
    {
        if(_upgrade.Value >= 7) { return; }

        int price = _upgrade.ShopCost * _upgrade.Value;

        if(_score.Value >= price)
        {
            _score.RemoveFromValue(price);
            _upgrade.AddToValue(1);
            SetNewPrice();
        }
    }

    void SetNewPrice()
    {
        if(_upgrade.Value >= 7)
        {
            _buttonPriceText.text = "SOLD OUT";
            _thisButton.interactable = false;
            return;
        }

        int price = _upgrade.Value * _upgrade.ShopCost;
        _buttonPriceText.text = $"{price}p";

        _thisButton.interactable = price <= _score.Value;        
    }
}
