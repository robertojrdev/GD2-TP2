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

    private MarketProduct product;

    private void Awake()
    {
        amountInputField.contentType = InputField.ContentType.IntegerNumber;
    }

    public void SetItemInfo(MarketProduct product)
    {
        this.product = product;
        icon.sprite = product.product.icon;
        textName.text = product.product.name;
        buyButton.onClick.AddListener(BuyItem);
        sellButton.onClick.AddListener(SellItem);
        amountInputField.text = product.basePurchasePrice.ToString();
    }

    private void BuyItem()
    {
        int amount = int.Parse(amountInputField.text);
        var isAboveMinimumPurchaseAmount = amount >= product.minimumPurchaseAmount;

        if (!isAboveMinimumPurchaseAmount)
        {
            print("you need do buy more in this market - " + product.minimumPurchaseAmount);
            return;
        }

        var price = amount * product.basePurchasePrice;
        var hasMoney = GameManager.Inventory.Money >= price;
        if (hasMoney)
        {
            GameManager.Inventory.AddMoney(-product.basePurchasePrice);
            GameManager.Inventory.AddItemSupply(product.product, amount);
        }
        else
        {
            print("you do not have enough money");
        }
    }

    private void SellItem()
    {
        int amount = int.Parse(amountInputField.text);
        var isAboveMinimumSaleAmount = amount >= product.minimumSaleAmount;

        if (!isAboveMinimumSaleAmount)
        {
            print("you need to sell more - " + product.minimumSaleAmount);
            return;
        }

        var hasEnoughProduct = GameManager.Inventory.GetItemFinalProductAmount(product.product) >= amount;
        if(!hasEnoughProduct)
        {
            print("you dont have enough of this to sell");
            return;
        }

        GameManager.Inventory.AddMoney(product.baseSellingPrice);
        GameManager.Inventory.RemoveItemFinalProduct(product.product, amount);
    }
}