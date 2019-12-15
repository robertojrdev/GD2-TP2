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
    }

    private void BuyItem()
    {
        int amount = int.Parse(amountInputField.text);
        
        var purchasesOnCurrentSeason = GameManager.Market.GetPurchaseAmount(marketIndex, marketProduct);
        var extrapolatedOffer = amount + purchasesOnCurrentSeason > marketProduct.offer;
        if(extrapolatedOffer)
        {
            print("you cannot buy this much of " + marketProduct.product.name + 
            ", this market only want to buy more " + 
            (marketProduct.demand - purchasesOnCurrentSeason));
            return;
        }


        var isAboveMinimumPurchaseAmount = amount >= marketProduct.minimumPurchaseAmount;
        if (!isAboveMinimumPurchaseAmount)
        {
            print("you need do buy more in this market - " + marketProduct.minimumPurchaseAmount);
            return;
        }

        var price = amount * marketProduct.basePurchasePrice;
        var hasMoney = GameManager.Inventory.Money >= price;
        if (hasMoney)
        {
            GameManager.Inventory.AddMoney(-marketProduct.basePurchasePrice * amount);
            GameManager.Inventory.AddItemSupply(marketProduct.product, amount);
        }
        else
        {
            print("you do not have enough money");
        }
    }

    private void SellItem()
    {
        int amount = int.Parse(amountInputField.text);

        var salesOnCurrentSeason = GameManager.Market.GetSalesAmount(marketIndex, marketProduct);
        var extrapolatedDemand = amount + salesOnCurrentSeason > marketProduct.demand;
        if(extrapolatedDemand)
        {
            print("you cannot sell this much, this market can only buy " + (marketProduct.demand - salesOnCurrentSeason));
            return;
        }

        var isAboveMinimumSaleAmount = amount >= marketProduct.minimumSaleAmount;

        if (!isAboveMinimumSaleAmount)
        {
            print("you need to sell more - " + marketProduct.minimumSaleAmount);
            return;
        }

        var hasEnoughProduct = GameManager.Inventory.GetItemFinalProductAmount(marketProduct.product) >= amount;
        if(!hasEnoughProduct)
        {
            print("you dont have enough of this to sell");
            return;
        }

        GameManager.Inventory.AddMoney(marketProduct.baseSellingPrice * amount);
        GameManager.Inventory.RemoveItemFinalProduct(marketProduct.product, amount);
    }
}