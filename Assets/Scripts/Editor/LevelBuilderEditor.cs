using Level;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelBuilder))]
    public class LevelBuilderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LevelBuilder merger = (LevelBuilder)target;
            if (GUILayout.Button("Preview Level"))
            {
                merger.PreviewLevel();
            }

            if (GUILayout.Button("Build Level"))
            {
                if (EditorUtility.DisplayDialog("Are you sure?", "This might fuck up your scene, make a backup!",
                    "I did, get on with it!", "I better commit first..."))
                {
                    merger.BuildLevel();
                }
            }
        }
    }
}