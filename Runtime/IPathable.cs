using System.Collections.Generic;
using UnityEngine;

namespace Hjelmqvist.Pathfinding
{
    public interface IPathable
    {
        List<IPathable> Connections { get; }
        Vector2Int Position { get; }
        bool IsWalkable();
    }
}