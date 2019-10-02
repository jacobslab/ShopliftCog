using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneManager : MonoBehaviour
{
    public List<GameObject> camZoneObjList;

    private CameraZone activeCamZone;
    // Start is called before the first frame update
    void Start()
    {
        SetCamZoneManagerReferences();
    }

    public void SetActiveCameraZone(CameraZone currentCamZone)
    {
        activeCamZone = currentCamZone;
    }

    public CameraZone GetActiveCameraZone()
    {
        return activeCamZone;
    }

    void SetCamZoneManagerReferences()
    {
        for(int i=0;i<camZoneObjList.Count;i++)
        {
            camZoneObjList[i].GetComponent<CameraZone>().camZoneManager = this.gameObject.GetComponent<CameraZoneManager>();
        }
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
        Debug.Log("should enable " + shouldEnable.ToString());
        for(int i=0;i< camZoneObjList.Count;i++)
        {
            camZoneObjList[i].GetComponent<CameraZone>().ToggleCamObjects(shouldEnable);
        }
    }

    public void MakeAllCamInvisible(bool isInvisible)
    {
        for(int i=0;i<camZoneObjList.Count;i++)
        {
            camZoneObjList[i].GetComponent<CameraZone>().MakeCamInvisible(isInvisible);
        }
    }

    public void ResetAllCamZones()
    {
        for(int i=0;i<camZoneObjList.Count;i++)
        {
            camZoneObjList[i].GetComponent<CameraZone>().Reset();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
