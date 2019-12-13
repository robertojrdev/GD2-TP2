using UnityEngine;

public class Item : MonoBehaviour
{

    public ItemInfo itemInfo { get; private set; }

    public void SetInfo(ItemInfo info)
    {
        itemInfo = info;
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}