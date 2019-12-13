using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory
{
    public int Money {get; private set;}
    public Dictionary<ItemInfo, int> Items { get; private set; } = new Dictionary<ItemInfo, int>();


    public static event Action<ItemInfo, int> onUpdateItemAmount;
    public static event Action<int> onUpdateMoney;

    /// <summary>
    /// can be used with negative values too
    /// </summary>
    /// <param name="clampToZero">will always keep the value equals or above 0</param>
    public void AddMoney(int amount, bool clampToZero = false)
    {
        Money += amount;
        if(clampToZero && amount < 0)
            Money = 0;
            
        if(onUpdateMoney != null)
            onUpdateMoney.Invoke(Money);
    }

    public void Add(ItemInfo item, int amount = 1)
    {
        if (!Items.ContainsKey(item))
            Items.Add(item, amount);
        else
            Items[item] += amount;

        if (onUpdateItemAmount != null)
            onUpdateItemAmount.Invoke(item, Items[item]);
    }

    public void Remove(ItemInfo item, int amount = 1)
    {
        if (Items.ContainsKey(item))
        {
            var current = Items[item];
            current -= amount;
            if (current <= 0)
            {
                current = 0;
                Items.Remove(item);
            }
            else
                Items[item] = current;

            if (onUpdateItemAmount != null)
                onUpdateItemAmount.Invoke(item, current);
        }
    }

    public int GetItemAmount(ItemInfo item)
    {
        if(Items.ContainsKey(item))
            return Items[item];
        
        return 0;
    }
}