using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{

    public Transform Eagle;
    private float vel = 0.0f;

    public void MapUp()
    {
        Eagle.position = Eagle.position + new Vector3(0f, 0f, 10f);
    }

    public void MapDown()
    {
        Eagle.position = Eagle.position + new Vector3(0f, 0f, -10f);
    }

    public void MapLeft()
    {
        Eagle.position = Eagle.position + new Vector3(-10f, 0f, 0f);
    }

    public void MapRight()
    {
        Eagle.position = Eagle.position + new Vector3(10f, 0f, 0f);
    }
    public void MapIn()
    {
        Eagle.position = Eagle.position + new Vector3(0f, -10f, 0f);
    }
    public void MapOut()
    {
        Eagle.position = Eagle.position + new Vector3(10f, 10f, 0f);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   
    }
}
