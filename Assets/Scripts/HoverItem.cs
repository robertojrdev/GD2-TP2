using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onHover;
    public UnityEvent onLeave;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onLeave.Invoke();
    }
}