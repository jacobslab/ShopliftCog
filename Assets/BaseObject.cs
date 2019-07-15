using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    public GameObject experimentObj;
    //camera
    public GameObject phase1CamZone_L;
    public GameObject phase1CamZone_R;
    public GameObject phase2CamZone_L;
    public GameObject phase2CamZone_R;
    public GameObject phase3CamZone_L;
    public GameObject phase3CamZone_R;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddCamZoneReferences(GameObject envPrefab)
    {
        envPrefab.GetComponent<EnvironmentManager>().phase1CamZone_L = phase1CamZone_L;
        envPrefab.GetComponent<EnvironmentManager>().phase1CamZone_R = phase1CamZone_R;
        envPrefab.GetComponent<EnvironmentManager>().phase2CamZone_L = phase2CamZone_L;
        envPrefab.GetComponent<EnvironmentManager>().phase2CamZone_R = phase2CamZone_R;
        envPrefab.GetComponent<EnvironmentManager>().phase3CamZone_L = phase3CamZone_L;
        envPrefab.GetComponent<EnvironmentManager>().phase3CamZone_R = phase3CamZone_R;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
