using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TaskManager : MonoBehaviour
{       
    public TaskData[] allTaskProcedures;
    public PersistentData persistentData; //player progress
    private string JsonDataFileName = "data.json";
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadJSONData();
        LoadPersistentData();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public TaskData GetCurrentProcedure()
    {
        return allTaskProcedures[0];
    }
    public void SubmitProceduresCompleted(int taskCompleted)
    {
       // sudo code until I figure this out
          if(taskCompleted>persistentData.proceduresCompleted)
          {
               persistentData.proceduresCompleted = taskCompleted;
               SavePersistentData();
          }
   
    }
    //for saving persistentData like what steps/procedures have been completed
    //lookinto PlayerPrefs
    public void LoadPersistentData()
    {   //playerprefs works like a dictionary
        persistentData = new PersistentData();
        if (PlayerPrefs.HasKey("procedureIndex"))
        {
            persistentData.proceduresCompleted = PlayerPrefs.GetInt("procedureIndex");
            //
        }
    }
    private void SavePersistentData()
    {
        PlayerPrefs.SetInt("procedureIndex", persistentData.proceduresCompleted);
    }
    public int GetHighestProcedureCompleted()
    {
        return persistentData.proceduresCompleted;
    }

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
