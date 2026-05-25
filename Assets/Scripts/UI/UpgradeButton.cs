using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UpgradeButton : MonoBehaviour
{
    public static event Action OnAnyUpgradePurchased;

    [SerializeField] IntReferenceSO _upgrade, _score;
    [SerializeField] TextMeshProUGUI _buttonPriceText;
    [SerializeField] FloatingText _floatingTextPrefab;

    void Awake()
    {
        Tigey.OnShopOpened += SetNewPrice;
        OnAnyUpgradePurchased += SetNewPrice;
    }

    void OnDestroy()
    {
        Tigey.OnShopOpened -= SetNewPrice;
        OnAnyUpgradePurchased -= SetNewPrice;
    }

    public void Purchase()  // UI Button
    {
        if(_upgrade.Value >= 7)
        {
            FloatingText floatingText = Instantiate(_floatingTextPrefab, transform.position, Quaternion.identity);
            floatingText.SetText("SOLD OUT!");
            return;
        }

        int price = _upgrade.ShopCost * (1 + _upgrade.Value);

        if(_score.Value >= price)
        {
            _score.RemoveFromValue(price);
            _upgrade.AddToValue(1);
            OnAnyUpgradePurchased?.Invoke();
        }
        else
        {
            FloatingText floatingText = Instantiate(_floatingTextPrefab, transform.position, Quaternion.identity);
            floatingText.SetText("TOO POOR!");
        }
    }

    void SetNewPrice()
    {
        if(_upgrade.Value >= 7)
        {
            _buttonPriceText.text = "MAX LEVEL";
            return;
        }

        int price = (_upgrade.Value + 1) * _upgrade.ShopCost;
        _buttonPriceText.text = $"{price}p";
    }
}
