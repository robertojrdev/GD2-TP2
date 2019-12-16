using UnityEngine;
using UnityEngine.UI;

public class PopUpMarketItemInfo : PopUpItem
{
    [SerializeField] private Text purchasePrice;
    [SerializeField] private Text salePrice;
    [SerializeField] private Text minimumPurchase;
    [SerializeField] private Text minimumSale;
    [SerializeField] private Text demand;
    [SerializeField] private Text offer;
    [SerializeField] private Text amountOnInventory;

    private int marketIndex;
    private MarketProduct marketProduct;

    private void OnDestroy()
    {
        Market.onPurchase -= UpdateOffer;
        Market.onSell -= UpdateDemand;
        Inventory.onUpdateItemFinalProductAmount -= UpdateStock;
    }

    public void SetItemInfo(MarketProduct marketProduct, int market)
    {
        purchasePrice.text = marketProduct.basePurchasePrice + "$";
        salePrice.text = marketProduct.baseSellingPrice + "$";
        minimumPurchase.text = marketProduct.minimumPurchaseAmount.ToString();
        minimumSale.text = marketProduct.minimumSaleAmount.ToString();
        demand.text = (marketProduct.demand - GameManager.Market.GetSalesAmount(market, marketProduct)).ToString();
        offer.text = (marketProduct.offer - GameManager.Market.GetPurchaseAmount(market, marketProduct)).ToString();
        amountOnInventory.text = GameManager.Inventory.GetItemFinalProductAmount(marketProduct.product).ToString();

        this.marketProduct = marketProduct;
        this.marketIndex = market;

        
        Market.onPurchase -= UpdateOffer;
        Market.onSell -= UpdateDemand;
        Inventory.onUpdateItemFinalProductAmount -= UpdateStock;

        Market.onPurchase += UpdateOffer;
        Market.onSell += UpdateDemand;
        Inventory.onUpdateItemFinalProductAmount += UpdateStock;
    }

    private void UpdateStock(ItemInfo item, int amount)
    {
        if(item == marketProduct.product)
            amountOnInventory.text = GameManager.Inventory.GetItemFinalProductAmount(marketProduct.product).ToString();
    }

    private void UpdateDemand(int marketIndex, MarketProduct marketProduct, int amount)
    {
        print("Called demand");

        if (marketIndex == this.marketIndex &&
            marketProduct == this.marketProduct)
        {
            print("pass demand");
            var salesAmount = GameManager.Market.GetSalesAmount(marketIndex, marketProduct);
            var demand = marketProduct.demand - salesAmount;
            print("sales amount: " + salesAmount + " - demand: " + demand);
            this.demand.text = demand.ToString();
        }
    }

    private void UpdateOffer(int marketIndex, MarketProduct marketProduct, int amount)
    {
        if (marketIndex == this.marketIndex &&
            marketProduct == this.marketProduct)
        {
            int purchasedAmount = GameManager.Market.GetPurchaseAmount(marketIndex, marketProduct);
            int offer = marketProduct.offer - purchasedAmount;
            this.offer.text = offer.ToString();
        }
    }
}