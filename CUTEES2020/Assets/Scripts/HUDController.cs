using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using SimpleJSON;
public class HUDController : MonoBehaviour
{
    
    //telemetry stream @ http://blooming-island-71601.herokuapp.com/api/simulation/state
    //public string j_url = "http://blooming-island-71601.herokuapp.com/api/simulation/state";

    public TaskManager taskManager;//holds  "dataController"
    



    public int Task_index = 0;
    public int Procedure_index = 0;
    public int Step_index = 0;
    public int Substep_index = 0;

    public JSONNode current_T;
    public JSONNode current_P;
    public JSONNode current_S;
    public JSONNode current_SS;





    public JSONNode taskboard;
    public Transform procedureListParent;
    
    private bool proceduresActive; //are there still procedures todo?
                           
    private int proceduresCompleted;
    private int procedureIndex; //which procedure are we on

    public GameObject ClickerCanvas;
    public GameObject ChecklistCanvas;
    public GameObject ChecklistCanvas2;

    private List<GameObject> stepsGameObjects = new List<GameObject>();
   
   
    public Text completedProcedures;
    public Text procedureText;
    public Text stepDisplayText;


    // Start is called before the first frame update
    void Start()
    {
        taskboard = taskManager.taskboard;
        current_T = taskboard["tasks"][Task_index];
        current_P = taskboard["tasks"][Task_index]["children"][Procedure_index];
        current_S = taskboard["tasks"][Task_index]["children"][Procedure_index]["children"][Step_index];
        current_SS = taskboard["tasks"][Task_index]["children"][Procedure_index]["children"][Step_index]["children"];
        var txb = ChecklistCanvas.GetComponent<Text>();
        var ttl = ClickerCanvas.GetComponent<Text>();
        txb.text = "";
        
        ttl.text = current_T["name"] + "- Current Step:  " + (Procedure_index + 1) + ". " + current_P["name"] + " - " + (Procedure_index + 1) + "." + (Step_index + 1) + ".  " + current_S["name"];
        for (var ii = 0; ii<current_SS.Count;ii++)//  stp in current_SS)
        {
            var stp = current_SS[ii]["action_object"] + " " + current_SS[ii]["condition"];
        
            txb.text = txb.text + " \n " + stp;
            
        }
}
    bool togg = true;
    public void toggle()
    {
        togg = !togg;
        ChecklistCanvas.SetActive(togg);
        ChecklistCanvas2.SetActive(togg);
    }
    
    public void next_step()
    {
        if (Step_index == current_P["childIds"][current_P["childIds"].Count - 1])
        {
            if (Procedure_index == current_T["childIds"][current_T["childIds"].Count - 1])
            {
                Task_index++;
                Procedure_index = taskboard["tasks"][Task_index]["childIds"][0];
                Step_index = taskboard["tasks"][Task_index]["childIds"][Procedure_index]["childIds"][0];
            }
            else
            {
                Procedure_index++;
                Step_index = taskboard["tasks"][Task_index]["childIds"][Procedure_index]["childIds"][0];
            }
        }
        else
        {
            Step_index++;
        }
    }
    public void previous_step()
    {
        if (Step_index == current_P["childIds"][0])
        {
            if (Procedure_index == current_T["childIds"][0])
            {
                Task_index--;
                Procedure_index = taskboard["tasks"][Task_index]["childIds"].Count - 1;
                Step_index = taskboard["tasks"][Task_index]["childIds"][Procedure_index]["childIds"].Count - 1;
            }
            else
            {
                Procedure_index++;
                Step_index = taskboard["tasks"][Task_index]["childIds"][Procedure_index]["childIds"].Count - 1;
            }
        }
        else
        {
            Step_index++;
        }
    }


    
    void Update()
    {
        taskboard = taskManager.taskboard;
        current_T = taskboard["tasks"][Task_index];
        current_P = taskboard["tasks"][Task_index]["children"][Procedure_index];
        current_S = taskboard["tasks"][Task_index]["children"][Procedure_index]["children"][Step_index];
        current_SS = taskboard["tasks"][Task_index]["children"][Procedure_index]["children"][Step_index]["children"];
        var txb = ChecklistCanvas.GetComponent<Text>();
        var ttl = ClickerCanvas.GetComponent<Text>();
        txb.text = "";

        ttl.text = current_T["name"] + "- Current Step:  " + (Procedure_index + 1) + ". " + current_P["name"] + " - " + (Procedure_index + 1) + "." + (Step_index + 1) + ".  " + current_S["name"];
        for (var ii = 0; ii < current_SS.Count; ii++)//  stp in current_SS)
        {
            var stp = current_SS[ii]["action_object"] + " " + current_SS[ii]["condition"];

            txb.text = txb.text + " \n " + stp;
            Debug.Log(current_SS.Count);
        }


    }


} 
