using UnityEngine;

public class ListItemGroup : MonoBehaviour
{
    private InventoryListItem _current;

    public void SetSelected(InventoryListItem item)
    {
        if(_current == item)
            return;

        if(_current)
            _current.SetSelected(false);

        _current = item;
    }
}