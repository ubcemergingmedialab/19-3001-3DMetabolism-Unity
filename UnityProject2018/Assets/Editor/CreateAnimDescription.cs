using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class CreateAnimDescription : EditorWindow
{
    public Object objectref;
    [MenuItem("Window/UI Toolkit/CreateAnimDescription")]
    public static void ShowExample()
    {
        CreateAnimDescription wnd = GetWindow<CreateAnimDescription>();
        wnd.titleContent = new GUIContent("Create a new Animation Description");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement buttonLabel = new Label("Press this button to generate an animation description");
        VisualElement button = new Button(CreateScriptableObject);
        root.Add(buttonLabel);
        root.Add(button);

        VisualElement textFieldLabel = new Label("Add triggers for the animation.");
        VisualElement triggerText = new TextField();
        VisualElement triggerAddButton = new Button(AddTrigger);
        root.Add(textFieldLabel);
        root.Add(triggerText);
        root.Add(triggerAddButton);

        IMGUIContainer dragDrop = new IMGUIContainer();
        dragDrop.onGUIHandler = () =>
        {
            objectref = EditorGUILayout.ObjectField(objectref, typeof(Object), true);
        };
        VisualElement dragDropText = new Label("Drag and drop objects.");
        VisualElement dragDropButton = new Button(AddObject);
        root.Add(dragDropText);
        root.Add(dragDrop);
        root.Add(dragDropButton);
        
        


        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CreateAnimDescription.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        // root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/CreateAnimDescription.uss");
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.styleSheets.Add(styleSheet);
        // root.Add(labelWithStyle);
    }

    public static void CreateScriptableObject()
    {
        AnimationDescription ad = ScriptableObject.CreateInstance<AnimationDescription>();
        AssetDatabase.CreateAsset(ad, "Assets/Scripts/ScriptableObjects/AnimDescription/animdescription.asset");
        AssetDatabase.SaveAssets();
        Debug.Log("SO created");
        Debug.Log(AssetDatabase.GetAssetPath(ad));
    }

    public static void AddTrigger()
    {
        Debug.Log("Add trigger");
    }

    public static void AddObject()
    {
        Debug.Log("Add Object");
    }
}