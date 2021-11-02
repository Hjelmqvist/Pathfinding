using System.Collections.Generic;
using UnityEngine;

namespace Hjelmqvist.Pathfinding
{
    public static class Pathfinding
    {
        public static class AStar
        {
            public static bool TryGetPath(IPathable[,] grid, Vector2Int startPosition, Vector2Int endPosition, Vector2Int[] neighborDirections, out List<Vector2Int> path)
            {
                if (!IsInsideBounds( grid, startPosition ) || !IsInsideBounds( grid, endPosition ))
                {
                    path = null;
                    return false;
                }

                PathNode startNode = new PathNode( startPosition, startPosition, endPosition, null );
                PathNode endNode = new PathNode( endPosition, startPosition, endPosition, null );
                List<PathNode> nodesToCheck = new List<PathNode>() { startNode };
                List<PathNode> checkedNodes = new List<PathNode>();

                while (nodesToCheck.Count > 0)
                {
                    PathNode currentNode = nodesToCheck[0];
                    int currentIndex = 0;
                    GetNodeWithLowestFCost( nodesToCheck, ref currentNode, ref currentIndex );
                    nodesToCheck.RemoveAt( currentIndex );
                    checkedNodes.Add( currentNode );

                    if (currentNode.Equals( endNode ))
                    {
                        path = currentNode.GetPath();
                        return true;
                    }
                    nodesToCheck.AddRange( GetNeighbors( currentNode, grid, startPosition, endPosition, neighborDirections, nodesToCheck, checkedNodes ) );
                }
                path = null;
                return false;
            }

            private static bool IsInsideBounds<T>(T[,] grid, Vector2Int position)
            {
                return position.x >= 0 && position.x < grid.GetLength( 0 ) &&
                       position.y >= 0 && position.y < grid.GetLength( 1 );
            }

            private static void GetNodeWithLowestFCost(List<PathNode> nodesToCheck, ref PathNode currentNode, ref int currentIndex)
            {
                for (int i = 0; i < nodesToCheck.Count; i++)
                {
                    if (nodesToCheck[i].F < currentNode.F)
                    {
                        currentNode = nodesToCheck[i];
                        currentIndex = i;
                    }
                }
            }

            private static List<PathNode> GetNeighbors(PathNode node, IPathable[,] grid, Vector2Int start, Vector2Int end, Vector2Int[] directions, List<PathNode> nodesToCheck, List<PathNode> checkedNodes)
            {
                List<PathNode> neighbors = new List<PathNode>();
                foreach (Vector2Int dir in directions)
                {
                    Vector2Int position = node.Position + dir;
                    PathNode newNode = new PathNode( position, start, end, node );

                    if (nodesToCheck.Contains( newNode ) || checkedNodes.Contains( newNode ) ||            // If the position hasn't been checked already
                        !IsInsideBounds( grid, position ) || !grid[position.x, position.y].IsWalkable()) // Is outside bounds or not walkable
                        continue;

                    neighbors.Add( newNode );
                }
                return neighbors;
            }
        }
    }
}