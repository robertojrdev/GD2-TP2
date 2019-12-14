using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "Farming/ItemInfo", order = 0)]
public class ItemInfo : ScriptableObject
{
    public ItemType type;
    public Sprite icon;
    public Item item;
    /// <summary>
    /// The amount planted on a single land
    /// </summary>
    public int amountPerLandUnit = 1;
    /// <summary>
    /// /// Time to be ready to harvest in seasons
    /// </summary>
    public int matureTime;
    [Range(0,1), SerializeField] private float WinterSuccessRate = 1;
    [Range(0,1), SerializeField] private float SpringSuccessRate = 1;
    [Range(0,1), SerializeField] private float SummerSuccessRate = 1;
    [Range(0,1), SerializeField] private float AutumnSuccessRate = 1;


    public float GetSuccessRate(Season season)
    {
        switch (season)
        {
            case Season.Winter:
                return WinterSuccessRate;
            case Season.Spring:
                return SpringSuccessRate;
            case Season.Summer:
                return SummerSuccessRate;
            case Season.Autumn:
                return AutumnSuccessRate;
        }

        throw new Exception("Not a season");
    }

}