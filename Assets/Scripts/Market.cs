
using UnityEngine;
using System;
using System.Collections.Generic;

public class Market
{
    private struct MarketMovement
    {
        public int marketIndex;
        public Dictionary<MarketProduct, int> purchases;
        public Dictionary<MarketProduct, int> sales;

        public MarketMovement(int marketIndex)
        {
            this.marketIndex = marketIndex;
            purchases = new Dictionary<MarketProduct, int>();
            sales = new Dictionary<MarketProduct, int>();
        }

        public void AddPurchase(MarketProduct marketProduct, int amount)
        {
            if (!purchases.ContainsKey(marketProduct))
                purchases.Add(marketProduct, amount);
            else
                purchases[marketProduct] += amount;
        }

        public void AddSale(MarketProduct marketProduct, int amount)
        {
            if (!sales.ContainsKey(marketProduct))
                sales.Add(marketProduct, amount);
            else
                sales[marketProduct] += amount;
        }

        public int GetPurchasesAmount(MarketProduct marketProduct)
        {
            if (purchases.ContainsKey(marketProduct))
                return purchases[marketProduct];

            return 0;
        }

        public int GetSalesAmount(MarketProduct marketProduct)
        {
            if (sales.ContainsKey(marketProduct))
                return sales[marketProduct];

            return 0;
        }
    }

    public static Action<int, MarketProduct, int> onSell;
    public static Action<int, MarketProduct, int> onPurchase;

    private List<MarketMovement> marketMovements = new List<MarketMovement>();

    private void AddPurchase(int marketIndex, MarketProduct marketProduct, int amount)
    {
        int movementId = FindMovementsIdByMarketIndex(marketIndex);
        marketMovements[movementId].AddPurchase(marketProduct, amount);
    }

    private void AddSale(int marketIndex, MarketProduct marketProduct, int amount)
    {
        int movementId = FindMovementsIdByMarketIndex(marketIndex);
        marketMovements[movementId].AddSale(marketProduct, amount);
    }

    private int FindMovementsIdByMarketIndex(int marketIndex)
    {
        var movementId = marketMovements.FindIndex(x => x.marketIndex == marketIndex);
        if (movementId == -1)
        {
            marketMovements.Add(new MarketMovement(marketIndex));
            movementId = marketMovements.Count - 1;
        }

        return movementId;
    }

    public int GetPurchaseAmount(int marketIndex, MarketProduct marketProduct)
    {
        int id = FindMovementsIdByMarketIndex(marketIndex);
        return marketMovements[id].GetPurchasesAmount(marketProduct);
    }

    public int GetSalesAmount(int marketIndex, MarketProduct marketProduct)
    {
        int id = FindMovementsIdByMarketIndex(marketIndex);
        return marketMovements[id].GetSalesAmount(marketProduct);
    }

    public void ResetMovements()
    {
        marketMovements.Clear();
    }

    public void Purchase(int marketIndex, MarketProduct marketProduct, int amount)
    {
        bool hasMoney = GameManager.Inventory.Money >= marketProduct.basePurchasePrice * amount;
        bool marketHasEnough = marketProduct.offer - GetPurchaseAmount(marketIndex, marketProduct) >= amount;
        bool isAboveMinimumPurchaseAmount = amount >= marketProduct.minimumPurchaseAmount;

        if (!hasMoney)
            Debug.Log("you have not enough money :(");

        if (!marketHasEnough)
            Debug.Log("market does not have this amount on sale");

        if(!isAboveMinimumPurchaseAmount)
            Debug.Log("this market has a minimum sale amount of " + marketProduct.minimumPurchaseAmount);

        if(!hasMoney || !marketHasEnough || !isAboveMinimumPurchaseAmount)
            return;

        GameManager.Inventory.AddMoney(-marketProduct.basePurchasePrice * amount);
        GameManager.Inventory.AddItemSupply(marketProduct.product, amount);
        AddPurchase(marketIndex, marketProduct, amount);

        if (onPurchase != null)
            onPurchase.Invoke(marketIndex, marketProduct, amount);
    }
    public void Sell(int marketIndex, MarketProduct marketProduct, int amount)
    {
        bool isOnDemand = marketProduct.demand - GetSalesAmount(marketIndex, marketProduct) >= amount;
        bool isAboveMinimumSellAmount = amount >= marketProduct.minimumSaleAmount;
        bool hasEnoughToSell = amount <= GameManager.Inventory.GetItemFinalProductAmount(marketProduct.product);

        if(!isOnDemand)
            Debug.Log("The market dont want to buy this much");

        if(!isAboveMinimumSellAmount)
            Debug.Log("The market has a minimum of " + marketProduct.minimumSaleAmount + " items per transaction");

        if(!hasEnoughToSell)
            Debug.Log("You dont have this much in stock");

        if(!isOnDemand || !isAboveMinimumSellAmount || !hasEnoughToSell)
            return;

        GameManager.Inventory.AddMoney(marketProduct.baseSellingPrice * amount);
        GameManager.Inventory.RemoveItemFinalProduct(marketProduct.product, amount);
        AddSale(marketIndex, marketProduct, amount);

        if (onSell != null)
            onSell.Invoke(marketIndex, marketProduct, amount);
    }
}