using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MarketMenuListItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text textName;
    [SerializeField] private InputField amountInputField;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private PopUpMarketItemInfo popUp;

    private int marketIndex;
    private MarketProduct marketProduct;

    private void Awake()
    {
        amountInputField.contentType = InputField.ContentType.IntegerNumber;
    }

    public void SetItemInfo(MarketProduct product, int marketIndex)
    {
        this.marketIndex = marketIndex;
        this.marketProduct = product;
        icon.sprite = product.product.icon;
        textName.text = product.product.name;
        buyButton.onClick.AddListener(BuyItem);
        sellButton.onClick.AddListener(SellItem);

        popUp.SetItemInfo(product, marketIndex);
    }

    private void BuyItem()
    {
        if(amountInputField.text == string.Empty)
            return;

        int amount = int.Parse(amountInputField.text);
        GameManager.Market.Purchase(marketIndex, marketProduct, amount);
    }

    private void SellItem()
    {
        if(amountInputField.text == string.Empty)
            return;
            
        int amount = int.Parse(amountInputField.text);
        GameManager.Market.Sell(marketIndex, marketProduct, amount);
    }
}