using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class GenerateSO : EditorWindow
{
    [MenuItem("Window/UI Toolkit/GenerateSO")]
    public static void ShowExample()
    {
        GenerateSO wnd = GetWindow<GenerateSO>();
        wnd.titleContent = new GUIContent("GenerateSO");
    }

    public static void CreateScriptableObject() {
        AnimationDescription ad = ScriptableObject.CreateInstance<AnimationDescription>();
        AssetDatabase.CreateAsset(ad, "Assets/Scripts/ScriptableObjects/AnimDescription");
        AssetDatabase.SaveAssets();
        Debug.Log("SO created");
        Debug.Log(AssetDatabase.GetAssetPath(ad));
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        VisualElement button = new Button(CreateScriptableObject);
        root.Add(button);
        root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/GenerateSO.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/GenerateSO.uss");
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);
    }
}