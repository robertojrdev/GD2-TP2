using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class LogItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text text;
    [SerializeField] private Image icon;
    [SerializeField] private float lifeTime = 6;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 3;

    [Space(10)]
    [SerializeField] private Color backColorInfo = Color.black;
    [SerializeField] private Color backColorSeason = Color.green;
    [SerializeField] private Color backColorWarning = Color.red;

    [Space(10)]
    [SerializeField] private Sprite iconDefault;
    [SerializeField] private Sprite iconSeason;
    [SerializeField] private Sprite iconWarning;
    [SerializeField] private Sprite iconHarvest;


    private CanvasGroup group;
    private Coroutine deathRoutine;
    private bool hovering = false;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    public void SetMessage(string msg, LogType type)
    {
        SetColor(type);
        SetIcon(type);
        text.text = msg;
        gameObject.SetActive(true);
    }

    private IEnumerator Start()
    {
        group.alpha = 0;

        while (group.alpha < 1)
        {
            group.alpha += Time.deltaTime / fadeInTime;
            yield return null;
        }

        if(!hovering)
            deathRoutine = StartCoroutine(Death());
    }

    private IEnumerator Death(float time = -1)
    {
        if(time == -1)
            yield return new WaitForSeconds(lifeTime);
        else
            yield return new WaitForSeconds(time);

        while (group.alpha > 0)
        {
            group.alpha -= Time.deltaTime / fadeOutTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void SetColor(LogType type)
    {
        var image = GetComponent<Image>();
        var finalColor = image.color;
        switch (type)
        {
            case LogType.Info:
                finalColor = Color.Lerp(finalColor, backColorInfo, 0.5f);
                break;
            case LogType.SeasonActions:
                finalColor = Color.Lerp(finalColor, backColorSeason, 0.5f);
                break;
            case LogType.Warning:
                finalColor = Color.Lerp(finalColor, backColorWarning, 0.5f);
                break;
        }

        image.color = finalColor;
    }

    private void SetIcon(LogType type)
    {
        switch (type)
        {
            case LogType.Info:
                icon.sprite = iconDefault;
                break;
            case LogType.SeasonActions:
                icon.sprite = iconSeason;
                break;
            case LogType.Warning:
                icon.sprite = iconWarning;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(deathRoutine != null)
            StopCoroutine(deathRoutine);

        group.alpha = 1;

        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;

        if(deathRoutine != null)
            StopCoroutine(deathRoutine);

        deathRoutine = StartCoroutine(Death(lifeTime / 2));
    }
}