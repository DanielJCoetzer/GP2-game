using UnityEditor;
using UnityEngine;

public class Floppy_Inspector : EditorWindow
{
    public new string floppyName = "Floppy Name";
    public int Id;
    public FloppyType floppyType;

    [MenuItem("Tools/Upgrades/Create Floppy")]
    public static void ShowWindow()
    {
        GetWindow(typeof(Floppy_Inspector));
    }

    private void OnGUI()
    {
        GUILayout.Label("Create new Enemy", EditorStyles.boldLabel);

        floppyName = EditorGUILayout.TextField("Enemy name", floppyName);
        Id = EditorGUILayout.IntField("Id", Id);
        floppyType = (FloppyType)EditorGUILayout.EnumPopup("Floppy type", floppyType); 

        if (GUILayout.Button("Create Floppy"))
        {
            CreateFloppy();
        }
    }

    void CreateFloppy()
    {
        SCR_SO_Ram floppy = CreateInstance<SCR_SO_Ram>();
        floppy.Id = Id;
        floppy.floppyType = floppyType;
        AssetDatabase.CreateAsset(floppy, $"Assets/RamSO/{floppyName}.asset");
        AssetDatabase.SaveAssets();
        

        GameObject obj = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/Prefabs/Controller/Game_Controller.prefab");

        if(floppy.floppyType == FloppyType.ESSENTIAL)
            obj.GetComponent<SCR_GameController>().CurrentEquippedRam.Add(floppy);
        else
            obj.GetComponent<SCR_GameController>().AvailableRam.Add(floppy);

        obj.GetComponent<SCR_GameController>().AllRam.Add(floppy);

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = obj;
    }
}
