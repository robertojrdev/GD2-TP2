using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Markets", menuName = "Farming/Markets", order = 0)]
public class MarketsScriptable : ScriptableObject 
{
    [System.Serializable]
    public struct Market
    {
        public string name;
        public List<MarketProduct> products;
    }

    [System.Serializable]
    public struct MarketProduct
    {
        public Item product;
        public int minimumPurchaseAmount;
        public int demand;
        public float basePurchasePrice;
        public float baseSellingPrice;
    }

    public List<Market> markets;
}