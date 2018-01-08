using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LevelDataMaker))]
public class BoothDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Export to xml", GUILayout.MaxWidth(200)))
        {
            Save();
        }
        //		Read from file
        if (GUILayout.Button("Read From xml", GUILayout.MaxWidth(200)))
        {
            Read();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    void Save()
    {
        string savePath = Application.dataPath + "/Resources/" + ((LevelDataMaker)target).fileName + ".xml";
        LevelDataCollection dataCollection = new LevelDataCollection();
        dataCollection.data = ((LevelDataMaker)target).levelData;

        dataCollection.Save(savePath);
        Debug.Log("Level Data saved to: " + savePath);
    }

    void Read()
    {
        string readPath = Application.dataPath + "/Resources/" + ((LevelDataMaker)target).fileName + ".xml";
        LevelDataCollection dataCollection = LevelDataCollection.Load(readPath);
        Debug.Log("Sponsor code Xml read from: " + readPath);
        ((LevelDataMaker)target).levelData = dataCollection.data;
    }

}


