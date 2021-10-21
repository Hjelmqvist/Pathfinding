using UnityEngine;

namespace Hjelmqvist.AStar.Sample
{
    [RequireComponent( typeof( MeshRenderer ) )]
    public class PathableTile : MonoBehaviour, IPathable
    {
        [SerializeField] bool _walkable = true;
        [SerializeField] Color _selectedColor = Color.magenta;

        Vector2Int _position;
        Color _defaultColor;

        public Vector2Int Position => _position;
        public MeshRenderer Renderer { get; private set; }

        static PathableTile from;

        public delegate void TileClicked(PathableTile from, PathableTile to);
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
                OnTileClicked.Invoke( from, this );
                from = null;
                return;
            }
            from = this;
            Renderer.material.color = _selectedColor;
        }

        void OnEnable()
        {
            PathableGrid.OnResetColors += PathableGrid_OnResetColors;
        }

        void OnDisable()
        {
            PathableGrid.OnResetColors -= PathableGrid_OnResetColors;
        }

        private void PathableGrid_OnResetColors()
        {
            Renderer.material.color = _defaultColor;
        }
    }
}