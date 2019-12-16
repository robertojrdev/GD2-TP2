using UnityEngine;

public class Log : MonoBehaviour
{
    private static Log instance;

    [SerializeField] private LogItem itemPrefab;

    public static void Msg(string msg, LogType type)
    {
        if(!instance)
        {
            Debug.Log("There is no Log instance...");
            return;
        }

        instance.Print(msg, type);
    }

    private void Awake() {
        if(!instance)
            instance = this;
        else
        {
            Debug.Log("Multiple Log instances not allowed");
            return;
        }
    }

    private void Print(string msg, LogType type)
    {
        var item = Instantiate(itemPrefab, transform);
        item.SetMessage(msg, type);
    }

}