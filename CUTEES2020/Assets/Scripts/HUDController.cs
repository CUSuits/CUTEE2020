using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUDController : MonoBehaviour
{
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
    {   //get task manager
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
            taskManager.SubmitProceduresCompleted(proceduresCompleted);
            EndProcedure();
            //this may not be necessary depending on what we want to do.
            DisplayMainHud();

        }
    }
    
        public void EndProcedure()
        {
            proceduresActive = false;
            taskManager.SubmitProceduresCompleted(proceduresCompleted);
            completedProcedures.text = taskManager.GetHighestProcedureCompleted().ToString();
            procedureCanvas.SetActive(false);


        }
        public void DisplayMainHud()
        {
            //several ways to do this lets discuss.
        }

       
    } 
