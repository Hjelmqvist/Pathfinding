using System.Collections.Generic;
using UnityEngine;

namespace Hjelmqvist.Pathfinding.Sample
{
    [RequireComponent( typeof( MeshRenderer ) )]
    public class SampleTile : MonoBehaviour, IPathable
    {
        [SerializeField] bool _walkable = true;
        [SerializeField] Color _selectedColor = Color.magenta;

        Vector2Int _position;
        Color _defaultColor;
        List<IPathable> _connections = new List<IPathable>();

        public Vector2Int Position => _position;
        public MeshRenderer Renderer { get; private set; }

        public List<IPathable> Connections { get { return _connections; } }

        static SampleTile from;

        public delegate void TileClicked(SampleTile from, SampleTile to);
        public static event TileClicked OnTileClicked;

        void Awake()
        {
            Renderer = GetComponent<MeshRenderer>();
            _defaultColor = Renderer.material.color;
        }

        public bool IsWalkable()
        {
            return _walkable;
        }

        public void SetPosition(Vector2Int position)
        {
            _position = position;
        }

        void OnMouseDown()
        {
            if (!_walkable)
                return;

            if (from)
            {
                OnTileClicked?.Invoke( from, this );
                from = null;
                return;
            }
            from = this;
            Renderer.material.color = _selectedColor;
        }

        void OnEnable()
        {
            SampleGrid.OnResetColors += PathableGrid_OnResetColors;
        }

        void OnDisable()
        {
            SampleGrid.OnResetColors -= PathableGrid_OnResetColors;
        }

        private void PathableGrid_OnResetColors()
        {
            Renderer.material.color = _defaultColor;
        }

        public void SetConnections(List<IPathable> pathables)
        {
            _connections = pathables;
        }
    }
}