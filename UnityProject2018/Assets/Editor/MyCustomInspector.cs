using UnityEditor;
using UnityEngine;
using System.Collections;
using MyScripts;
using MyCustomEditor;
[InitializeOnLoad]
[ExecuteInEditMode]
[CustomEditor(typeof(MyCustomScript))]
public class MyCustomInspector : Editor
{

    private GUIContent guiContent;
    private MyCustomScript myTarget;
    private static string layer = SomeClass.Layer;
    void OnEnable()
    {
        myTarget = (MyCustomScript)target;
        layer = myTarget.layer = SomeClass.Layer;
        TagsAndLayers.AddLayer(layer);
    }
    
    public override void OnInspectorGUI()
    {
        string tmpString;
        myTarget = (MyCustomScript)target;
        EditorGUI.BeginChangeCheck();
        guiContent = new GUIContent("Layer Name", "Set the name of the layer");
        tmpString = EditorGUILayout.TextField(guiContent, myTarget.layer);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(myTarget, "Layer Name");
            myTarget.layer = tmpString;
            if (layer != null && myTarget.layer != "")
            {
                if (myTarget.layer != layer)
                {
                    TagsAndLayers.RemoveLayer(layer);
                }
                TagsAndLayers.AddLayer(myTarget.layer);
                layer = SomeClass.Layer = myTarget.layer;
            }
        }
        EditorUtility.SetDirty(myTarget);
    }
    void OnInspectorUpdate()
    {
        this.Repaint();
    }
}