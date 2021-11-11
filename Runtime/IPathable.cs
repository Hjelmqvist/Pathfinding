using System.Collections.Generic;
using UnityEngine;

namespace Hjelmqvist.Pathfinding
{
    public interface IPathable
    {
        Vector2Int Position { get; }
        bool IsWalkable();
        List<IPathable> GetConnections();
        void SetConnections(List<IPathable> pathables);
    }
}