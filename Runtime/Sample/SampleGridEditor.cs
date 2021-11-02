using UnityEditor;
using UnityEngine;

namespace Hjelmqvist.Pathfinding.Sample
{
    [CustomEditor( typeof( SampleGrid ) )]
    public class SampleGridEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button( "Create new grid" ))
            {
                SampleGrid grid = (SampleGrid)target;
                if (grid)
                    grid.CreateGrid();
            }
        }
    }
}