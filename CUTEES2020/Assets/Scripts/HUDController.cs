using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUDController : MonoBehaviour
{
    /**
     * state:
    Received: {"heart_bpm":60,
        "p_sub":"2.49",
        "p_suit":"3.94",
        "t_sub":"5","v_fan":"39034",
        "p_o2":"942","rate_o2":"0.9",
        "cap_battery":"30",
        "p_h2o_g":"15",
        "p_h2o_l":"16",
        "p_sop":"942",
        "rate_sop":"1.0",
        "t_battery":
        "-5:-10:-50",
        "t_oxygen":"8:35:1",
        "t_water":"8:35:1",
        "createDate":
        "Fri Feb 14 2020 03:36:47 GMT+0000 (Coordinated Universal Time)"}
    UnityEngine.Debug:Log(Object)
    <GetRequest>d__20:MoveNext() (at Assets/Scripts/HUDController.cs:129)
    UnityEngine.SetupCoroutine:InvokeMoveNext(IEnumerator, IntPtr) (at C:/buildslave/unity/build/Runtime/Export/Coroutines.cs:17)
        **/
    //telemetry stream @ http://blooming-island-71601.herokuapp.com/api/simulation/state
    

    private TaskManager taskManager;//holds  "dataController"
    private TaskData currentProcedure;//
    private Procedure[] procedureClass;//
   
    public Transform procedureListParent;
    
    private bool proceduresActive; //are there still procedures todo?
                           
    private int proceduresCompleted;
    private int procedureIndex; //which procedure are we on
   
    public GameObject procedureCanvas;
    
    private List<GameObject> stepsGameObjects = new List<GameObject>();
   
    public StepObjectPool stepPoolObject;
   
    public Text completedProcedures;
    public Text procedureText;
    public Text stepDisplayText;
   
    
    // Start is called before the first frame update
    void Start()
    {    
       //get task manager
        //get current proccedure
        //getarray of steps
        taskManager = FindObjectOfType<TaskManager>();
        currentProcedure = taskManager.GetCurrentProcedure();
        procedureClass = currentProcedure.allTask;
       
        ShowProcedure();
        proceduresActive = true;
    }

    private void ShowProcedure()
    {

        RemovePreviousProcedureSteps();
        Procedure procedureData = procedureClass[procedureIndex];
        procedureText.text = procedureData.procedure;//get procedure name for text element

        //for each step spawn a textbox
        for (int i = 0; i < procedureData.stepList.Length; i++)
        {
            //create gameobject (GO); set inside the procedure panel GO
            GameObject stepPoolObjectObject = stepPoolObject.GetObject();
            stepPoolObjectObject.transform.SetParent(procedureListParent, false);
            //add GO to array of pooled objects
            stepsGameObjects.Add(stepPoolObjectObject);
            //add text to GO
            StepText steptext = stepPoolObjectObject.GetComponent<StepText>();
            steptext.Setup(procedureData.stepList[i]);
        }
    }

    private void RemovePreviousProcedureSteps()
    {   //remove spawn pooled steps.
        while (stepsGameObjects.Count > 0)
        {
            stepPoolObject.ReturnObject(stepsGameObjects[0]);
            stepsGameObjects.RemoveAt(0);
        }

    }
    public void NextStep()
    {
        //for now click button but eventually voice command
        // when clicked next step mark prcedure as done 
        if (procedureClass.Length > procedureIndex + 1)
        {
            procedureIndex++;
            ShowProcedure();
            proceduresCompleted += 1;
        }
        else
        {
            proceduresCompleted += 1;
            
            EndProcedure();
            //this may not be necessary depending on what we want to do.
            DisplayMainHud();

        }
    }
    
        public void EndProcedure()
        {
            proceduresActive = false;
            

            procedureCanvas.SetActive(false);


        }
        public void DisplayMainHud()
        {
            //several ways to do this lets discuss.
        }
    

} 
