using UnityEngine;

[CreateAssetMenu(fileName = "InitialSetup", menuName = "Farming/InitialSetup", order = 0)]
public class InitialSetup : ScriptableObject
{
    [System.Serializable]
    public struct ItemAmount
    {
        public ItemInfo itemInfo;
        public int amount;
    }

    public int money = 500;
    public Vector2Int mapSize = Vector2Int.one * 100;
    public Vector2Int acquiredLands = new Vector2Int(5, 5);
    public ItemAmount[] initialItems;

}