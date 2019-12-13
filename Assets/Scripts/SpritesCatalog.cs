using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpritesCatalog", menuName = "Farming/SpritesCatalog", order = 0)]
public class SpritesCatalog : ScriptableObject
{
    private const string CATALOG_PATH = "Game Settings\\SpritesCatalog";


    [System.Serializable]
    public class SpriteKeyPair : KeyPair<Sprite> { }


    private static SpritesCatalog instance;


    [SerializeField] private List<SpriteKeyPair> sprites;
    public List<SpriteKeyPair> Sprites { get => sprites; }


    public static Sprite Get(string key)
    {
        if (!instance)
        {
            instance = Resources.Load(CATALOG_PATH) as SpritesCatalog;
            if (!instance)
            {
                Debug.Log("SPRITE CATALOG NOT FOUND!\nit must be on Resources folder at " + CATALOG_PATH);
                return null;
            }
        }

        key = key.ToLower();
        var sprite  = instance.Sprites?.Find(x => x.key.ToLower() == key)?.value;
        return sprite;
    }
}