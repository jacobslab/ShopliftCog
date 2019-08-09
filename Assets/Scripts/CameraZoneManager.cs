using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneManager : MonoBehaviour
{
    public List<GameObject> camZoneObjList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetCameraObjects()
    {
        for (int i = 0; i < camZoneObjList.Count; i++)
        {
            camZoneObjList[i].GetComponent<CameraZone>().SetCameraObject();
        }
    }

    public void ToggleAllCamZones(bool shouldEnable)
    {
        for(int i=0;i< camZoneObjList.Count;i++)
        {
            camZoneObjList[i].GetComponent<CameraZone>().ToggleCamObjects(shouldEnable);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
