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

    public List<Market> markets;
}