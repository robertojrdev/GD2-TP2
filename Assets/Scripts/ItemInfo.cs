using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "Farming/ItemInfo", order = 0)]
public class ItemInfo : ScriptableObject
{
    public ItemType type;
    public Sprite icon;
    public Item item;
}