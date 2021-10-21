using System.Collections.Generic;
using UnityEngine;
using Hjelmqvist.AStar;

public class PathableGrid : MonoBehaviour
{
    [SerializeField] PathableTile _walkable, _notWalkable;
    [SerializeField] int _rows, _columns;
    [SerializeField] Vector2Int[] directions =
                    {
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 1),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, -1),
                        new Vector2Int(0, -1),
                        new Vector2Int(-1, -1),
                        new Vector2Int(-1, 0),
                        new Vector2Int(-1, 1)
                    };

    PathableTile[,] _tiles;
    Transform _gridParent;

    public delegate void ResetColors();
    public static event ResetColors OnResetColors;

    void Start()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        if (_gridParent)
            Destroy(_gridParent.gameObject);

        _gridParent = new GameObject("Grid").transform;
        _gridParent.SetParent(transform);
        _tiles = new PathableTile[_rows, _columns];
        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _columns; y++)
            {
                PathableTile prefab = Random.value < 0.8f ? _walkable : _notWalkable;
                _tiles[x, y] = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, _gridParent);
                _tiles[x, y].SetPosition(new Vector2Int(x, y));
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2Int end = new Vector2Int(_rows - 1, _columns - 1);

            if (Pathfinding.TryGetPath(_tiles, Vector2Int.zero, end, directions, out List<Vector2Int> path))
            {
                Debug.Log("Found path");
                foreach (Vector2Int pos in path)
                {
                    if (_tiles[pos.x, pos.y].TryGetComponent(out MeshRenderer renderer))
                        renderer.material.color = Color.blue;
                }
            }
            else
                Debug.Log("Failed to find path");
        }
    }

    private void OnEnable()
    {
        PathableTile.OnTileClicked += PathableTile_OnTileClicked;
    }
    private void OnDisable()
    {
        PathableTile.OnTileClicked -= PathableTile_OnTileClicked;
    }

    private void PathableTile_OnTileClicked(PathableTile from, PathableTile to)
    {
        OnResetColors.Invoke();
        if (Pathfinding.TryGetPath(_tiles,from.Position, to.Position, directions, out List<Vector2Int> path))
        {
            Debug.Log("Found path");
            foreach (Vector2Int pos in path)
                _tiles[pos.x, pos.y].Renderer.material.color = Color.blue;
        }
        else
            Debug.Log("Failed to find path");
    }
}
