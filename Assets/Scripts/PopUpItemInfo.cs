using UnityEngine;
using UnityEngine.UI;

public class PopUpItemInfo : PopUpItem
{
    [SerializeField] private Text matureTime;
    [SerializeField] private Text successRateWinter;
    [SerializeField] private Text successRateSpring;
    [SerializeField] private Text successRateSummer;
    [SerializeField] private Text successRateAutumn;

    public void SetItemInfo(ItemInfo itemInfo)
    {
        matureTime.text = itemInfo.matureTime + "s";
        successRateWinter.text = itemInfo.GetSuccessRate(Season.Winter).ToString();
        successRateSpring.text = itemInfo.GetSuccessRate(Season.Spring).ToString();
        successRateSummer.text = itemInfo.GetSuccessRate(Season.Summer).ToString();
        successRateAutumn.text = itemInfo.GetSuccessRate(Season.Autumn).ToString();
    }

}