using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hjelmqvist.Pathfinding
{
    public class PathNode : IEquatable<PathNode>
    {
        IPathable _pathable;
        float _g;
        float _h;
        PathNode _parent;

        public IPathable Pathable => _pathable;
        public float F => _g + _h;
        public PathNode Parent => _parent;

        public PathNode(IPathable pathable, Vector2Int startPosition, Vector2Int endPosition, PathNode parent)
        {
            _pathable = pathable;
            _g = Vector2Int.Distance( pathable.Position, startPosition );
            _h = Vector2Int.Distance( pathable.Position, endPosition );
            _parent = parent;
        }

        public bool Equals(PathNode other)
        {
            return _pathable.Position == other.Pathable.Position;
        }

        public List<Vector2Int> GetPath()
        {
            List<Vector2Int> path = new List<Vector2Int>();
            PathNode current = this;
            while (current != null)
            {
                path.Add( current.Pathable.Position );
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}