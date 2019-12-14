using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level Instance { get; private set; }


    [SerializeField] private Land landTile;
    [SerializeField] private Vector2 landTileSize = Vector2.one;
    [SerializeField] private LayerMask landLayer = 0;


    private Transform _levelHolder;
    private static Land[,] _landField;
    private static Vector2 _mapSize;

    public static Vector3 Center => Instance ?
        new Vector3(_mapSize.x * Instance.landTileSize.x / 2, 0, _mapSize.y * Instance.landTileSize.y / 2) :
        Vector3.zero;


    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
        {
            Debug.LogError("Multiple levels in the scene", gameObject);
            return;
        }

        if (!_levelHolder)
            _levelHolder = new GameObject("Level Holder").transform;

    }

    public void DrawLevel(Vector2Int mapSize, Vector2Int acquiredLands)
    {
        _mapSize = mapSize;

        _landField = new Land[mapSize.x, mapSize.y];

        var startPos = mapSize / 2 - acquiredLands / 2;
        var endPos = startPos + acquiredLands;

        var offset = new Vector3(mapSize.x / 2 * landTileSize.x, 0, mapSize.y / 2 * landTileSize.y);

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                var position = new Vector3(x * landTileSize.x, 0, y * landTileSize.y) - offset;
                var tile = Instantiate(landTile, position, Quaternion.identity, _levelHolder);
                _landField[x, y] = tile;

                var acquired =
                    x >= startPos.x &&
                    x < endPos.x &&
                    y >= startPos.y &&
                    y < endPos.y;

                if (acquired)
                    tile.Acquire();
            }
        }
    }

    public static Land GetLandAtMousePosition(Camera camera)
    {
        if (!Instance)
            return null;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, Instance.landLayer))
        {
            var land = hit.transform.GetComponent<Land>();
            if (land != null)
                return land;
        }

        return null;
    }

}
