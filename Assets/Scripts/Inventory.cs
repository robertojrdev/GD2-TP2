using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory
{
    public int Money { get; private set; }
    public Dictionary<ItemInfo, int> ItemsSupply { get; private set; } = new Dictionary<ItemInfo, int>();
    public Dictionary<ItemInfo, int> ItemsFinalProduct { get; private set; } = new Dictionary<ItemInfo, int>();


    public static event Action<ItemInfo, int> onUpdateItemSupplyAmount;
    public static event Action<ItemInfo, int> onUpdateItemFinalProductAmount;
    public static event Action<int> onUpdateMoney;

    /// <summary>
    /// can be used with negative values too
    /// </summary>
    /// <param name="clampToZero">will always keep the value equals or above 0</param>
    public void AddMoney(int amount, bool clampToZero = false)
    {
        Money += amount;
        if (clampToZero && amount < 0)
            Money = 0;

        if (onUpdateMoney != null)
            onUpdateMoney.Invoke(Money);
    }

    public void AddItemSupply(ItemInfo item, int amount = 1)
    {
        if (!ItemsSupply.ContainsKey(item))
            ItemsSupply.Add(item, amount);
        else
            ItemsSupply[item] += amount;

        if (onUpdateItemSupplyAmount != null)
            onUpdateItemSupplyAmount.Invoke(item, ItemsSupply[item]);
    }

    public void RemoveItemSupply(ItemInfo item, int amount = 1)
    {
        if (ItemsSupply.ContainsKey(item))
        {
            var current = ItemsSupply[item];
            current -= amount;
            if (current <= 0)
            {
                current = 0;
                ItemsSupply.Remove(item);
            }
            else
                ItemsSupply[item] = current;

            if (onUpdateItemSupplyAmount != null)
                onUpdateItemSupplyAmount.Invoke(item, current);
        }
    }

    public void AddItemFinalProduct(ItemInfo item, int amount = 1)
    {
        if (!ItemsFinalProduct.ContainsKey(item))
            ItemsFinalProduct.Add(item, amount);
        else
            ItemsFinalProduct[item] += amount;

        if (onUpdateItemFinalProductAmount != null)
            onUpdateItemFinalProductAmount.Invoke(item, ItemsFinalProduct[item]);
    }

    public void RemoveItemFinalProduct(ItemInfo item, int amount = 1)
    {
        if (ItemsFinalProduct.ContainsKey(item))
        {
            var current = ItemsFinalProduct[item];
            current -= amount;
            if (current <= 0)
            {
                current = 0;
                ItemsFinalProduct.Remove(item);
            }
            else
                ItemsFinalProduct[item] = current;

            if (onUpdateItemFinalProductAmount != null)
                onUpdateItemFinalProductAmount.Invoke(item, current);
        }
    }

    public int GetItemSupplyAmount(ItemInfo item)
    {
        if (ItemsSupply.ContainsKey(item))
            return ItemsSupply[item];

        return 0;
    }

    public int GetItemFinalProductAmount(ItemInfo item)
    {
        if (ItemsFinalProduct.ContainsKey(item))
            return ItemsFinalProduct[item];

        return 0;
    }
}