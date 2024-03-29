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

    private void Start() {
        transform.localScale = Vector3.one * 0.2f;
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
                age++;
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

        var deltaSize = Mathf.InverseLerp(0, itemInfo.matureTime, age);
        var size = Mathf.Lerp(0.2f, 1, deltaSize);
        transform.localScale = Vector3.one * size;
    }

    public void Harvest(Season season)
    {
        var successRate = itemInfo.GetSuccessRate(season);
        var dice = UnityEngine.Random.Range(0f, 1f);

        if (dice <= successRate)
        {
            GameManager.Inventory.AddItemFinalProduct(itemInfo, itemInfo.amountPerLandUnit);
            Log.Msg("Harvested " + itemInfo.amountPerLandUnit + " of " + itemInfo.name, LogType.SeasonActions);
        }
        else
        {
            Log.Msg("A plantation of " + itemInfo.name + " failed", LogType.Warning);
        }

        Destroy(gameObject);
    }
}