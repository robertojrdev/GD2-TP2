using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuChooser : MonoBehaviour
{
    [System.Serializable]
    public struct MenuButton
    {
        public Button button;
        public RectTransform menu;
        public CanvasGroup group;
        public Vector2 openPivotPosition;
        public Vector2 closedPivotPosition;
    }

    [SerializeField] private MenuButton[] labels;

    private int _currentOpened = -1;

    private void Awake()
    {
        for (int i = 0; i < labels.Length; i++)
        {
            int id = i;
            labels[i].button?.onClick.AddListener(() => Display(id));
        }
    }

    private void Display(int id)
    {
        if (id == _currentOpened)
            Close(id);
        else
        {
            if (_currentOpened != -1)
                Close(_currentOpened);

            _currentOpened = id;
            labels[id].button.GetComponent<Image>().color = Color.green;
            StartCoroutine(DisplayAnimation(true, id));
        }
    }

    private IEnumerator DisplayAnimation(bool visible, int id)
    {
        labels[id].menu.gameObject.SetActive(true);
        var initialPos = visible ? labels[id].closedPivotPosition : labels[id].openPivotPosition;
        var finalPos = visible ? labels[id].openPivotPosition : labels[id].closedPivotPosition;
        var initialAlpha = visible ? 0 : 1;
        var finalAlpha = visible ? 1 : 0;

        var animationTime = 0.25f;
        var time = 0f;

        while (time < animationTime)
        {
            time += Time.deltaTime;
            var delta = Mathf.InverseLerp(0, animationTime, time);

            var pivot = Vector2.Lerp(initialPos, finalPos, delta);
            var alpha = Mathf.Lerp(initialAlpha, finalAlpha, delta);

            labels[id].menu.pivot = pivot;
            labels[id].group.alpha = alpha;
            yield return null;
        }

        labels[id].menu.pivot = finalPos;
        labels[id].group.alpha = finalAlpha;
        labels[id].menu.gameObject.SetActive(visible);

    }

    private void Close(int id)
    {
        labels[id].button.GetComponent<Image>().color = Color.white;
        StartCoroutine(DisplayAnimation(false, id));
        _currentOpened = -1;
    }
}