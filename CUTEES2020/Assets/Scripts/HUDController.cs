using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using SimpleJSON;
using TMPro;
public class HUDController : MonoBehaviour
{


    public JSONmanager jsonmanager;//holds  "dataController"
    public A_Star_Pathfinder Astar;



    public int Task_index = 0;
    public int Procedure_index = 0;
    public int Step_index = 0;
    public int Substep_index = 0;

    public JSONNode current_T;
    public JSONNode current_P;
    public JSONNode current_S;
    public JSONNode current_SS;


    public JSONNode taskboard;
    public JSONNode suit_rep;
    public Transform procedureListParent;

    public TextMeshProUGUI time_canvas;
    public TextMeshProUGUI Procedure_canvas;
    public TextMeshProUGUI Step_canvas;
    public TextMeshProUGUI Path_deets;
    public TextMeshProUGUI Stats;
    public TextMeshProUGUI Warning;
    public TextMeshProUGUI Caution;
    public RadialGauge O2_val;
    public RadialGauge SOP_val;
    public RadialGauge bat_val;

    public List<bool> telem_warnings = new List<bool>{false, false, false, false, false, false, false, false, false};
    float start_time;
    // Start is called before the first frame update
    void Start()
    {
        taskboard = jsonmanager.taskboard;
        current_T = taskboard["tasks"][Task_index];
        current_P = taskboard["tasks"][Task_index]["children"][Procedure_index];
        current_S = taskboard["tasks"][Task_index]["children"][Procedure_index]["children"][Step_index];
        current_SS = taskboard["tasks"][Task_index]["children"][Procedure_index]["children"][Step_index]["children"];
        //var txb = ChecklistCanvas.GetComponent<Text>();
        //var ttl = ClickerCanvas.GetComponent<Text>();
        start_time = Time.time;
    }
    private int togg = 0;
    public void toggle()
    {
        togg++;
        if (togg > 4) togg = 0;
        //ChecklistCanvas.SetActive(togg);
        //ChecklistCanvas2.SetActive(togg);
    }

    public void next_step()
    {
        Step_index++;
    }
    public void previous_step()
    {
        Step_index--;
    }
    public void next_p()
    {
        Procedure_index++;
        Task_index = 0;
        Step_index = 0;
        Substep_index = 0;
    }
    public void previous_p()
    {
        Procedure_index--;
    }
    public void next_t()
    {
        Task_index++;
        Task_index = 0;
        Step_index = 0;
        Substep_index = 0;
    }
    public void previous_t()
    {
        Task_index--;
    }



    void Update()
    {
        
        // Access the Tasklist (stored in JSONManajer script - taskboard)
        taskboard = jsonmanager.taskboard;
        suit_rep = jsonmanager.suit_rep;
        check_telem();
        //update current task/step/procedure based on the step
        current_T = taskboard["tasks"][Task_index];
        current_P = taskboard["tasks"][Task_index]["children"][Procedure_index];
        var p_p = taskboard["tasks"][Task_index]["children"][Procedure_index - 1];
        var xnt_p = taskboard["tasks"][Task_index]["children"][Procedure_index + 1];
        current_S = taskboard["tasks"][Task_index]["children"][Procedure_index]["children"][Step_index];
        var p_s = taskboard["tasks"][Task_index]["children"][Procedure_index]["children"][Step_index -1 ];
        var xnt_s = taskboard["tasks"][Task_index]["children"][Procedure_index]["children"][Step_index + 1];
        current_SS = taskboard["tasks"][Task_index]["children"][Procedure_index]["children"][Step_index]["children"];
        var time_rem = suit_rep[0];
        Procedure_canvas.text = (Procedure_index) + ". " + p_p["name"] + "\n\n" + (Procedure_index + 1) + ". " + current_P["name"] + "\n\n" + (Procedure_index + 2) + ". " + xnt_p["name"] + "\n\n";
        Step_canvas.text = (Procedure_index+1) + "."+(Step_index) + " " + p_s["object"] + " " + p_s["action"] + "\n\n" + (Procedure_index + 1) + "." + (Step_index+1) + " "+ current_S["object"] + " " + current_S["action"] + "\n\n" + (Procedure_index + 1) + "." + (Step_index+2) + " " + xnt_s["object"]+ " " + xnt_s["action"];

        var elapsed = (Time.time - start_time);
        var remaining = 8 * 60 * 60 - elapsed;
        time_canvas.text = "Elapsed Time:       " + System.TimeSpan.FromSeconds((int)elapsed) +"\n"+ "Remaining Time:   " + System.TimeSpan.FromSeconds((int)remaining) + "\n" + "Current Task: " + current_T["name"];

        Path_deets.text = "Estimated Time: " + (int) (Astar.path_time/60f) + "min \n\n\nEstimated Distance: " + (int) Astar.path_dist +"m";

        var ts_rem = "Oxygen Life: " + suit_rep[0]["t_oxygen"] + "\nWater Life: " + suit_rep[0]["t_water"] + "\nBattery Life: " + suit_rep[0]["t_battery"];

        var elec = " Electrical Stats: \n\nBattery Capacity: " + suit_rep[0]["cap_battery"];
        var thermal = "Thermal Stats: \n\nOutside Temp: " + suit_rep[0]["t_sub"];
        var fluid = "Fluid Stats: \n\nH2O liquid Pressure: " + suit_rep[0]["p_h2o_l"];
        var Atm = "Atmosphere Stats: \n\nInternal Suit Pressure: " + suit_rep[0]["p_suit"] +
            "\nExternal Pressure: " + suit_rep[0]["p_sub"] +
            "\nFan RPM: " + suit_rep[0]["v_fan"] +
            "\nPrimary Oxygen Pressure: " + suit_rep[0]["p_o2"] +
            "\nPrimary Oxygen rate: " + suit_rep[0]["rate_o2"]  +
            "\nSecondary Oxygen Pressure: " + suit_rep[0]["p_sop"]  +
            "\nSecondary Oxygen rate: " + suit_rep[0]["rate_sop"] +
            "\nH2O gas Pressure: " + suit_rep[0]["p_h2o_g"];

        var allStats = "All Stats\n\nHeart BPM: "+ suit_rep[0]["heart_bpm"] + "\t\t\t" +  
            "Internal Suit Pressure: " + suit_rep[0]["p_suit"] + 
            "\nFAN TACHOMETER: " + suit_rep[0]["v_fan"] + "\t\t\t" +
            "Primary Oxygen Pressure: " + suit_rep[0]["p_o2"] + 
            "\nPrimary Oxygen rate: " + suit_rep[0]["rate_o2"] + "\t\t\t" +
            "Battery Capacity: " + suit_rep[0]["cap_battery"] + 
            "\nH2O gas Pressure: " + suit_rep[0]["p_h2o_g"] + "\t\t\t" +
            "H2O liquid Pressure: " + suit_rep[0]["p_h2o_l"] + 
            "\nSecondary Oxygen Pressure: " + suit_rep[0]["p_sop"] + "\t\t\t" +
            "Secondary Oxygen rate: " + suit_rep[0]["rate_sop"];

        if (togg == 0)
        {
            Stats.text = ts_rem + "\n\n" + allStats;
        }
        if (togg == 1)
        {
            Stats.text = ts_rem + "\n\n" + Atm;
        }
        if (togg == 2)
        {
            Stats.text = ts_rem + "\n\n" + fluid;
        }
        if (togg == 3)
        {
            Stats.text = ts_rem + "\n\n" + thermal;
        }
        if (togg == 4)
        {
            Stats.text = elec;
        }

        O2_val.currentValue = (suit_rep[0]["p_o2"] - 750f) / 2f;
        SOP_val.currentValue = (suit_rep[0]["p_sop"] - 750f) / 2f;
        bat_val.currentValue = (suit_rep[0]["cap_battery"]) / 0.3f;


    }








    public void check_telem()
    {
        if ((suit_rep[0]["p_suit"]<2f || suit_rep[0]["p_suit"]>4f) && !telem_warnings[0])
        {
            Warning.transform.parent.gameObject.SetActive(true);
            telem_warnings[0] = true;
            Warning.text += "\nInternal Suit Pressure: " + suit_rep[0]["p_suit"];
        }
        if ((suit_rep[0]["v_fan"] < 10000f || suit_rep[0]["v_fan"] > 40000f) && !telem_warnings[1])
        {
            Warning.transform.parent.gameObject.SetActive(true);
            telem_warnings[1] = true;
            Warning.text += "\nFAN TACHOMETER: " + suit_rep[0]["v_fan"];
        }
        if ((suit_rep[0]["p_o2"] < 750f || suit_rep[0]["p_o2"] > 950f) && !telem_warnings[2])
        {
            Warning.transform.parent.gameObject.SetActive(true);
            telem_warnings[2] = true;
            Warning.text += "\nPrimary Oxygen Pressure: " + suit_rep[0]["p_o2"];
        }
        if ((suit_rep[0]["rate_o2"] < 0.5f || suit_rep[0]["rate_o2"] > 1f) && !telem_warnings[3])
        {
            Warning.transform.parent.gameObject.SetActive(true);
            telem_warnings[3] = true;
            Warning.text += "\nPrimary Oxygen rate: " + suit_rep[0]["rate_o2"];
        }
        if ((suit_rep[0]["cap_battery"] <= 0f || suit_rep[0]["cap_battery"] > 30f) && !telem_warnings[4])
        {
            Warning.transform.parent.gameObject.SetActive(true);
            telem_warnings[4] = true;
            Warning.text += "\nBattery Capacity: " + suit_rep[0]["cap_battery"];
        }
        if ((suit_rep[0]["p_h20_g"] < 14f || suit_rep[0]["p_h2o_g"] > 16f) && !telem_warnings[5])
        {
            Warning.transform.parent.gameObject.SetActive(true);
            telem_warnings[5] = true;
            Warning.text += "\nH2O gas Pressure: " + suit_rep[0]["p_h2o_g"];
        }
        if ((suit_rep[0]["p_h20_l"] < 14f || suit_rep[0]["p_h2o_l"] > 16f) && !telem_warnings[6])
        {
            Warning.transform.parent.gameObject.SetActive(true);
            telem_warnings[6] = true;
            Warning.text += "\nH2O liquid Pressure: " + suit_rep[0]["p_h2o_l"];
        }
        if ((suit_rep[0]["p_sop"] < 750f || suit_rep[0]["p_sop"] > 950f) && !telem_warnings[7])
        {
            Warning.transform.parent.gameObject.SetActive(true);
            telem_warnings[7] = true;
            Warning.text += "\nSecondary Oxygen Pressure: " + suit_rep[0]["p_sop"];
        }
        if ((suit_rep[0]["rate_sop"] < 0.5f || suit_rep[0]["rate_sop"] > 1f) && !telem_warnings[8])
        {
            Warning.transform.parent.gameObject.SetActive(true);
            telem_warnings[8] = true;
            Warning.text += "\nSecondary Oxygen rate: " + suit_rep[0]["rate_sop"];
        }


    }

    public void hide_warnings()
    {
        Warning.transform.parent.gameObject.SetActive(false);
    }
    public void clear_warnings()
    {
        Warning.text += "Warning!";
        telem_warnings = new List<bool>{ false,false,false,false,false,false,false,false};
    }

    public void hide_caution()
    {
        Caution.transform.parent.gameObject.SetActive(false);
    }

    public void hide_procedure()
    {
        Procedure_canvas.transform.parent.gameObject.SetActive(false);
    }
    public void hide_step()
    {
        Step_canvas.transform.parent.gameObject.SetActive(false);
    }
    public void show_stats()
    {
        Stats.transform.parent.gameObject.SetActive(true);
        SOP_val.gameObject.SetActive(true);
        O2_val.gameObject.SetActive(true);
        bat_val.gameObject.SetActive(true);
    }
    public void hide_stats()
    {
        Stats.transform.parent.gameObject.SetActive(false);
        SOP_val.gameObject.SetActive(false);
        O2_val.gameObject.SetActive(false);
        bat_val.gameObject.SetActive(false);
    }





    public void hideAll()
    {
        Procedure_canvas.transform.parent.gameObject.SetActive(false);
        Step_canvas.transform.parent.gameObject.SetActive(false);
        Path_deets.transform.parent.gameObject.SetActive(false);
        Stats.transform.parent.gameObject.SetActive(false);
        SOP_val.gameObject.SetActive(false);
        O2_val.gameObject.SetActive(false);
        bat_val.gameObject.SetActive(false);
    }


    public void showTasks()
    {
        hideAll();
        Procedure_canvas.transform.parent.gameObject.SetActive(true);
        Step_canvas.transform.parent.gameObject.SetActive(true);
    }






}

