using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MarketMenu : MonoBehaviour
{
    [SerializeField] private MarketsScriptable markets;
    [SerializeField] private Dropdown marketsDropdown;
    [SerializeField] private RectTransform productsListHolder;
    [SerializeField] private MarketMenuListItem productsListItemPrefab;

    private void Start()
    {
        marketsDropdown.onValueChanged.AddListener(x =>
        {
            ClearMarketList();
            FillMarketList();
        });
    }

    private void OnEnable()
    {
        ClearMarketList();
        FillDropdown();
        FillMarketList();
    }

    public void FillDropdown()
    {
        marketsDropdown.ClearOptions();
        var marketNames = markets.markets.Select(x => x.name).ToList();
        marketsDropdown.AddOptions(marketNames);
        marketsDropdown.value = 0;
    }

    public void FillMarketList()
    {
        int marketIndex = marketsDropdown.value;

        foreach (var marketProduct in markets.markets[marketIndex].products)
        {
            var item = Instantiate(productsListItemPrefab);
            item.transform.SetParent(productsListHolder, false);
            item.SetItemInfo(marketProduct);
            item.gameObject.SetActive(true);
        }
    }

    public void ClearMarketList()
    {
        int count = productsListHolder.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = productsListHolder.GetChild(i);

            if (child == productsListItemPrefab.transform)
                continue;

            Destroy(child.gameObject);
        }
    }
}
