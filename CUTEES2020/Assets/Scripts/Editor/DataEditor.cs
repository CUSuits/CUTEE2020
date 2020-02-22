using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
/* DataEditor script */
public class DataEditor : EditorWindow
{
    private string JSONDataProjectFilePath = "/StreamingAssets/data.json";
    public JSONData jSONData;
    
    [MenuItem("Window/Game Data Editor")]
    static void init()
    {
        EditorWindow.GetWindow(typeof(DataEditor)).Show();
    }

    void OnGUI()
    {
        if (jSONData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("jSONData");
            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save data"))
            {
                SaveJSONData();
            }
        }

        if (GUILayout.Button("Load data"))
        {
            LoadJSONData();
        }
    }

    private void LoadJSONData()
    {
        string filePath = Application.dataPath + JSONDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            jSONData = JsonUtility.FromJson<JSONData>(dataAsJson);
        }
        else
        {
            
            jSONData = new JSONData();
        }
    }
    
    private void SaveJSONData()
    {
        string dataAsJson = JsonUtility.ToJson(jSONData);
        string filepath = Application.dataPath + JSONDataProjectFilePath;
        File.WriteAllText(filepath, dataAsJson);
    }


}
