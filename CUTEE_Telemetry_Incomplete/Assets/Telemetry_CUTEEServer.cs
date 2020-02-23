using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class NumericalTelemetry
{
    public int heart_bpm;       //Heart beats per minute
    public float p_sub;         //Sub pressure
    public float p_suit;        //Suit pressure
    public float t_sub;         //Sub temperature
    public int v_fan;           //Fan tachometer
    public float p_o2;          //Oxygen pressure
    public float rate_o2;       //Oxygen rate
    public float cap_battery;   //Batter capacity
    public float p_h2o_g;       //H2O gas pressure
    public float p_h2o_l;       //H2O liquid pressure
    public float p_sop;         //Secondary oxygen pack pressure
    public float rate_sop;      //Oxygen rate for secondary pack
    public string t_battery;    //Battery time
    public string t_oxygen;     //Oxygen time
    public string t_water;      //Water time

}

[System.Serializable]
public class SwitchTelemetry
{
    public bool sop_on;         //Secondary oxygen pack is active
    public bool sspe;           //Spacesuit pressure emergency
    public bool fan_error;      //Cooling fan failure
    public bool vent_error;     //No vent flow
    public bool vehicle_power;  //Receiving power through spacecraft
    public bool h2o_off;        //H2O system is offline
    public bool o2_off;         //O2 system is offline
}

//Numerical Data Points
public class SuitDataPoints
{
    public string name;
    public object value;


    public SuitDataPoints(string name, object value)
    {
        this.name = name;
        this.value = value;
    }
}

public class SwitchDataPoints
{
    public string name;
    public object status;

    public SwitchDataPoints(string name, object status)
    {
        this.name = name;
        this.status = status;
    }
}


public class Telemetry_CUTEEServer : MonoBehaviour
    {
        public NumericalTelemetry SuitStats;
        // Start is called before the first frame update
        void Start()
        {
            // A correct website page.
            StartCoroutine(GetStats("http://blooming-island-71601.herokuapp.com/api/simulation/state"));
            //Might need to add another coroutine for the switch data

    }

    void Update()
        {
        }


        IEnumerator GetStats(string uri)
        {
            while (true)
            {
                using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
                {
                    // Request and wait for the desired page.
                    yield return webRequest.SendWebRequest();


                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(webRequest.error);
                    }
                    else
                    {
                        string jsonString = webRequest.downloadHandler.text;
                        jsonString = jsonString.Replace('[', ' ').Replace(']', ' ');
                        SuitStats = JsonUtility.FromJson<NumericalTelemetry>(jsonString);
                        Debug.Log(SuitStats.heart_bpm);
                        Debug.Log(SuitStats.p_sub);

                        int numStats= 10; //Number of components

                        SuitDataPoints[] suitDataPoints = new SuitDataPoints[numStats];


                }
            }
            }
        }

    //Sets Suit Telemetry
    public void setSuitData(NumericalTelemetry SuitStats, SuitDataPoints[] dataPoints)
    {
        dataPoints[0] = new SuitDataPoints("Heart BPM", SuitStats.heart_bpm);
        dataPoints[1] = new SuitDataPoints("Sub Presure", SuitStats.p_sub);
        dataPoints[2] = new SuitDataPoints("Suit pressure", SuitStats.p_suit);
        dataPoints[3] = new SuitDataPoints("Sub Temperature", SuitStats.t_sub);
        dataPoints[4] = new SuitDataPoints("Fan Temperature", SuitStats.v_fan);
        dataPoints[5] = new SuitDataPoints("Oxygen pressure", SuitStats.p_o2);
        dataPoints[6] = new SuitDataPoints("Oxygen Rate", SuitStats.rate_o2);
        dataPoints[7] = new SuitDataPoints("Batery Capacity", SuitStats.cap_battery);
        dataPoints[8] = new SuitDataPoints("H2O gas pressure", SuitStats.p_h2o_g);
        dataPoints[9] = new SuitDataPoints("H2O liquid pressure", SuitStats.p_h2o_l);
        dataPoints[5] = new SuitDataPoints("Secondary oxygen pack pressure", SuitStats.p_sop);
        dataPoints[6] = new SuitDataPoints("Oxygen rate for secondary pack", SuitStats.rate_sop);
        dataPoints[7] = new SuitDataPoints("Battery time", SuitStats.t_battery);
        dataPoints[8] = new SuitDataPoints("Oxygen time", SuitStats.t_oxygen);
        dataPoints[9] = new SuitDataPoints("Water time", SuitStats.t_water);
    }

    public void setSwitchData(SwitchTelemetry SuitStats, SwitchDataPoints[] dataPoints)
    {
        dataPoints[0] = new SwitchDataPoints("SOP On", SuitStats.sop_on);
        dataPoints[1] = new SwitchDataPoints("Spacesuit Pressure Emergency", SuitStats.sspe);
        dataPoints[2] = new SwitchDataPoints("Fan Failure", SuitStats.fan_error);
        dataPoints[3] = new SwitchDataPoints("No Vent Flow", SuitStats.vent_error);
        dataPoints[4] = new SwitchDataPoints("Vehicle Power Present", SuitStats.vehicle_power);
        dataPoints[5] = new SwitchDataPoints("H2O is off", SuitStats.h2o_off);
        dataPoints[6] = new SwitchDataPoints("O2 is off", SuitStats.o2_off);
    }
    public class SwitchTelemetry
    {
        public bool sop_on;         //Secondary oxygen pack is active
        public bool sspe;           //Spacesuit pressure emergency
        public bool fan_error;      //Cooling fan failure
        public bool vent_error;     //No vent flow
        public bool vehicle_power;  //Receiving power through spacecraft
        public bool h2o_off;        //H2O system is offline
        public bool o2_off;         //O2 system is offline
    }

}


