using System.Windows;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;
using SimpleJSON;
public class TaskManager : MonoBehaviour
{       //datacontroller
    public telemetry telemet;
    
    public string j_url = "http://blooming-island-71601.herokuapp.com/api/simulation/state";
    public string t_url = "https://nova-eva-support-cutee2020.herokuapp.com/api/utils/procedures";

    int i = 0;
    public JSONNode taskboard;
    void Start()
    {
    
        StartCoroutine(GetRequest(t_url));
       
    }
    public GameObject load;
    void Update()
    {
        if (taskboard != null)
        {
            load.SetActive(false);
        }
        Debug.Log(taskboard == null);
        if (i%720 == 0)
        {
            StartCoroutine(GetRequest(t_url));
        }
        i++;
    }


    public string jdat;
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
                jdat = File.ReadAllText(Application.dataPath + "/task_backup.json");

            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                jdat = webRequest.downloadHandler.text;
                taskboard = JSON.Parse(jdat);
            }
        }
    }
}

