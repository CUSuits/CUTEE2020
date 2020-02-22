using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TaskManager : MonoBehaviour
{       //datacontroller
    public TaskData[] allTaskProcedures;

    private string JsonDataFileName = "data.json";
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadJSONData();
      
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public TaskData GetCurrentProcedure()
    {
        return allTaskProcedures[0];
    }
  
    //for saving persistentData like what steps/procedures have been completed
    //lookinto PlayerPrefs
   
  
  

    private void LoadJSONData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, JsonDataFileName);
        if (File.Exists(filePath))
        {
            string dataAsJSON = File.ReadAllText(filePath); //reads file
            JSONData loadedJSONData = JsonUtility.FromJson<JSONData>(dataAsJSON);//makes object
            
            allTaskProcedures = loadedJSONData.allTaskData;
        }
        else
        {
            Debug.LogError("Cannot Load Game Data");
        }
    }

}
