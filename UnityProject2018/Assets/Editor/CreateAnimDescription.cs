using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class CreateAnimDescription : EditorWindow
{
    public static Object objectref;
    public static List<string> triggerList = new List<string>();
    public static List<string> objectList = new List<string>();
    public static string triggerString = "";
    public static TextField triggerText = new TextField();

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
        Button button = new Button(CreateScriptableObject);
        button.text = "Create scriptable Object";
        root.Add(button);

        Label textFieldLabel = new Label("Add triggers for the animation.");
        // TextField triggerText = new TextField();
        Button triggerAddButton = new Button(AddTrigger);
        triggerAddButton.text = "Click this to add a trigger";
        root.Add(textFieldLabel);
        root.Add(triggerText);
        root.Add(triggerAddButton);

        IMGUIContainer dragDrop = new IMGUIContainer();
        dragDrop.onGUIHandler = () =>
        {
            objectref = EditorGUILayout.ObjectField(objectref, typeof(Object), true);
        };
        VisualElement dragDropText = new Label("Drag and drop objects.");
        Button dragDropButton = new Button(AddObject);
        dragDropButton.text = "Click this to add an object.";
        root.Add(dragDropText);
        root.Add(dragDrop);
        root.Add(dragDropButton);

        Label createText = new Label("Create the scriptable object");
        root.Add(createText);
        root.Add(button);

        Button triggerDisplay = new Button(displayTriggers);
        triggerDisplay.text = "Display triggers added";
        Button objectDisplay = new Button(displayObjects);
        objectDisplay.text = "Display objects added";

        root.Add(triggerDisplay);
        root.Add(objectDisplay);

        Button clearTriggerList = new Button(clearTrigger);
        clearTriggerList.text = "Clear trigger list";
        Button clearObjectList = new Button(clearObject);
        clearObjectList.text = "Clear object list";

        root.Add(clearTriggerList);
        root.Add(clearObjectList);



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

    public static void displayTriggers()
    {
        Debug.Log("List of triggers added");
        foreach (var v in triggerList)
        {
            Debug.Log(v);
        };
    }

    public static void displayObjects()
    {
        Debug.Log("List of objects added");
        foreach (var v in objectList)
        {
            Debug.Log(v);
        };
    }

    public static void CreateScriptableObject()
    {
        AnimationDescription ad = ScriptableObject.CreateInstance<AnimationDescription>();
        ad.AnimatedObjects = objectList;
        ad.TriggerToSet = triggerList;
        AssetDatabase.CreateAsset(ad, "Assets/Scripts/ScriptableObjects/AnimDescription/animdescription");
        AssetDatabase.SaveAssets();
        Debug.Log("SO created");
        Debug.Log(AssetDatabase.GetAssetPath(ad));
    }

    public static void AddTrigger()
    {
        // triggerList.Add(triggerString);
        // Debug.Log(triggerString);
        Debug.Log(triggerText.text);
        triggerList.Add(triggerText.text);
        Debug.Log("Add trigger");
        foreach (var v in triggerList)
        {
            Debug.Log(v);
        }
    }

    public static void AddObject()
    {
        objectList.Add(objectref.name);
        Debug.Log(objectref.name);
        Debug.Log("Add Object");
        foreach (var v in objectList)
        {
            Debug.Log(v);
        }
    }

    public static void clearTrigger()
    {
        triggerList = new List<string>();
    }

    public static void clearObject()
    {
        objectList = new List<string>();
    }
}