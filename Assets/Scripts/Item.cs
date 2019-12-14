using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemInfo itemInfo { get; private set; }

    public ItemStage stage { get; private set; } = ItemStage.Planting;
    public int age { get; private set; } = 0; //age is counted by seasons

    public void SetInfo(ItemInfo info)
    {
        itemInfo = info;
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }

    public void AdvanceSeason(Season season)
    {
        switch (stage)
        {
            case ItemStage.Planting:
                stage = ItemStage.Growing;
                break;
            case ItemStage.Growing:
                age++;

                if (age >= itemInfo.matureTime)
                {
                    stage = ItemStage.ReadyToHarvest;
                    Harvest(season);
                }
                break;
        }
    }

    public void Harvest(Season season)
    {
        var successRate = itemInfo.GetSuccessRate(season);
        var dice = UnityEngine.Random.Range(0f, 1f);

        if (dice <= successRate)
        {
            GameManager.Inventory.AddItemFinalProduct(itemInfo, itemInfo.amountPerLandUnit);
        }

        Destroy(gameObject);
    }
}