using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer gridBackground;  
    [SerializeField] private Transform gridParent;     

    [Header("Level Prefabs")]
    [SerializeField] private GameObject redCubePrefab;
    [SerializeField] private GameObject greenCubePrefab;
    [SerializeField] private GameObject blueCubePrefab;
    [SerializeField] private GameObject yellowCubePrefab;
    [SerializeField] private GameObject rocketVPrefab;
    [SerializeField] private GameObject rocketHPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private GameObject vasePrefab;

    private Dictionary<string, GameObject> _prefabMap;
    private int _width, _height;

    public void InitGrid(LevelData data)
    {
        _width = data.grid_width;
        _height = data.grid_height;

        _prefabMap = new Dictionary<string, GameObject>
        {
            { "r", redCubePrefab },
            { "g", greenCubePrefab },
            { "b", blueCubePrefab },
            { "y", yellowCubePrefab },
            { "vro", rocketVPrefab },
            { "hro", rocketHPrefab },
            { "bo", boxPrefab },
            { "s", stonePrefab },
            { "v", vasePrefab },
        };
        
        float tileSize = redCubePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        
        Vector2 boardSize = new Vector2(_width, _height) * tileSize;
        gridBackground.drawMode = SpriteDrawMode.Sliced;
        gridBackground.size = boardSize;
        gridBackground.transform.position = new Vector3(0f, -2.8f, 0f);

        Vector2 gridOrigin = (Vector2)gridBackground.transform.position - boardSize * 0.5f;

        for (int i = gridParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(gridParent.GetChild(i).gameObject);

        for (int i = 0; i < data.grid.Count; i++)
        {
            int x = i % _width;
            int y = i / _width;
            string key = data.grid[i];

            if (key == "rand")
            {
                var options = new[] { "r", "g", "b", "y" };
                key = options[Random.Range(0, options.Length)];
            }

            if (!_prefabMap.TryGetValue(key, out GameObject prefab) || prefab == null)
            {
                Debug.LogWarning($"[GridManager] No prefab found for key '{key}'");
                continue;
            }

            Vector2 spawnPos = gridOrigin + new Vector2((x + 0.5f), (y + 0.5f)) * tileSize;
            GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity, gridParent);
        }
    }
}