using System.Collections.Generic;
using UnityEngine;

namespace Hjelmqvist.Pathfinding
{
    public static class Pathfinding
    {
        public static class AStar
        {
            public static bool TryGetPath(IPathable start, IPathable end, out List<Vector2Int> path)
            {
                PathNode startNode = new PathNode( start, start.Position, end.Position, null );
                PathNode endNode = new PathNode( end, start.Position, end.Position, null );
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
                    nodesToCheck.AddRange( GetNeighbors( currentNode, start.Position, end.Position, nodesToCheck, checkedNodes ) );
                }
                path = null;
                return false;
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

            private static List<PathNode> GetNeighbors(PathNode node, Vector2Int start, Vector2Int end, List<PathNode> nodesToCheck, List<PathNode> checkedNodes)
            {
                List<PathNode> neighbors = new List<PathNode>();
                foreach (IPathable pathable in node.Pathable.Connections)
                {
                    PathNode newNode = new PathNode( pathable, start, end, node );

                    if (nodesToCheck.Contains( newNode ) || checkedNodes.Contains( newNode ) || !pathable.IsWalkable())
                        continue;
                    neighbors.Add( newNode );
                }
                return neighbors;
            }
        }
    }
}