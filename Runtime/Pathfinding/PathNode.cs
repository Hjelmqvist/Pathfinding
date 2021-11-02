using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hjelmqvist.Pathfinding
{
    public class PathNode : IEquatable<PathNode>
    {
        Vector2Int _position;
        float _g;
        float _h;
        PathNode _parent;

        public Vector2Int Position => _position;
        public float F => _g + _h;
        public PathNode Parent => _parent;

        public PathNode(Vector2Int position, Vector2Int startPosition, Vector2Int endPosition, PathNode parent)
        {
            _position = position;
            _g = Vector2Int.Distance( position, startPosition );
            _h = Vector2Int.Distance( position, endPosition );
            _parent = parent;
        }

        public bool Equals(PathNode other)
        {
            return _position == other._position;
        }

        public List<Vector2Int> GetPath()
        {
            List<Vector2Int> path = new List<Vector2Int>();
            PathNode current = this;
            while (current != null)
            {
                path.Add( current.Position );
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}