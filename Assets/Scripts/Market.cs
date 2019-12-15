
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
            if(!purchases.ContainsKey(marketProduct))
                purchases.Add(marketProduct, amount);
            else
                purchases[marketProduct] += amount;
        }

        public void AddSale(MarketProduct marketProduct, int amount)
        {
            if(!sales.ContainsKey(marketProduct))
                sales.Add(marketProduct, amount);
            else
                sales[marketProduct] += amount;
        }

        public int GetPurchasesAmount(MarketProduct marketProduct)
        {
            if(purchases.ContainsKey(marketProduct))
                return purchases[marketProduct];
            
            return 0;
        }

        public int GetSalesAmount(MarketProduct marketProduct)
        {
            if(sales.ContainsKey(marketProduct))
                return sales[marketProduct];
            
            return 0;
        }
    }

    private List<MarketMovement> marketMovements = new List<MarketMovement>();

    public void AddPurchase(int marketIndex, MarketProduct marketProduct, int amount)
    {
        int movementId = FindMovementsIdByMarketIndex(marketIndex);
        marketMovements[movementId].AddPurchase(marketProduct, amount);
    }
    
    public void AddSale(int marketIndex, MarketProduct marketProduct, int amount)
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
        return marketMovements[id].GetPurchasesAmount(marketProduct);
    }

    public void ResetMovements()
    {
        marketMovements.Clear();
    }
}