using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Hjelmqvist.Pathfinding.Sample
{
    public class SampleGrid : MonoBehaviour
    {
        [SerializeField] SampleTile _walkable, _notWalkable;
        [SerializeField] int _rows, _columns;

        SampleTile[,] _tiles;
        Transform _gridParent;
        const string GRID_NAME = "Grid";

        public delegate void ResetColors();
        public static event ResetColors OnResetColors;

        void Start()
        {
            _gridParent = transform.Find( GRID_NAME );
            CreateGrid();
            SetConnections();
        }

        public void CreateGrid()
        {
            if (_gridParent)
            {
#if UNITY_EDITOR
                DestroyImmediate( _gridParent.gameObject );
#else
                Destory( _gridParent.gameObject );
#endif
            }

            _gridParent = new GameObject( GRID_NAME ).transform;
            _gridParent.SetParent( transform );
            _tiles = new SampleTile[_rows, _columns];
            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _columns; y++)
                {
                    SampleTile prefab = Random.value < 0.8f ? _walkable : _notWalkable;
                    _tiles[x, y] = Instantiate( prefab, new Vector3( x, 0, y ), Quaternion.identity, _gridParent );
                    _tiles[x, y].SetPosition( new Vector2Int( x, y ) );
                }
            }
        }

        private void SetConnections()
        {
            Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, 
                                  new Vector2Int(1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1), new Vector2Int(-1, 1) };

            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _columns; y++)
                {
                    List<IPathable> connections = new List<IPathable>();
                    foreach (Vector2Int dir in dirs)
                    {
                        Vector2Int pos = _tiles[x, y].Position + dir;
                        if (pos.x >= 0 && pos.x < _rows && pos.y >= 0 && pos.y < _columns)
                            connections.Add( _tiles[pos.x, pos.y] );
                    }
                    _tiles[x, y].SetConnections( connections );
                }
            }
        }

        void Update()
        {
            if (Input.GetKeyDown( KeyCode.Space ))
            {
                Vector2Int end = new Vector2Int( _rows - 1, _columns - 1 );

                if (Pathfinding.AStar.TryGetPath( _tiles[0, 0], _tiles[end.x, end.y], out List<Vector2Int> path ))
                {
                    UnityEngine.Debug.Log( "Found path" );
                    foreach (Vector2Int pos in path)
                    {
                        if (_tiles[pos.x, pos.y].TryGetComponent( out MeshRenderer renderer ))
                            renderer.material.color = Color.blue;
                    }
                }
                else
                    UnityEngine.Debug.Log( "Failed to find path" );
            }
        }

        private void OnEnable()
        {
            SampleTile.OnTileClicked += PathableTile_OnTileClicked;
        }
        private void OnDisable()
        {
            SampleTile.OnTileClicked -= PathableTile_OnTileClicked;
        }

        private void PathableTile_OnTileClicked(SampleTile from, SampleTile to)
        {
            OnResetColors.Invoke();
            Stopwatch watch = new Stopwatch();
            watch.Start();
    
            if (Pathfinding.AStar.TryGetPath( from, to, out List<Vector2Int> path ))
            {
                watch.Stop();
                UnityEngine.Debug.Log( "Pathfinding time: " + (watch.ElapsedMilliseconds) );
                UnityEngine.Debug.Log( "Found path" );
                foreach (Vector2Int pos in path)
                    _tiles[pos.x, pos.y].Renderer.material.color = Color.blue;
            }
            else
                UnityEngine.Debug.Log( "Failed to find path" );
        }
    }
}