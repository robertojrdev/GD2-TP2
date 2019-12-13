using UnityEngine;
using UnityEngine.UI;

public class MarketMenuListItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text textName;

    public string ProductName { get => textName.text; set => textName.text = value; }
}