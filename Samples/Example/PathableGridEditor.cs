using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( PathableGrid ) )]
public class PathableGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button( "Create new grid" ))
        {
            PathableGrid grid = (PathableGrid)target;
            if (grid)
                grid.CreateGrid();
        }
    }
}