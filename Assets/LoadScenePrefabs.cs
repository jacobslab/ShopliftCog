using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class LoadScenePrefabs: MonoBehaviour
{

    private string[] assetBundleNames = new string[4];
    private GameObject basePrefabObj;
    private GameObject experimentObj;
    private List<GameObject> scenePrefabList= new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        basePrefabObj = LoadPrefabFromAssetBundle("base");
        experimentObj = basePrefabObj.GetComponent<BaseObject>().experimentObj;

        assetBundleNames[0] = "spacestation";
        assetBundleNames[1] = "office";
        for (int i = 0; i < 2; i++)
        {
            GameObject prefab=LoadPrefabFromAssetBundle(assetBundleNames[i]);
            scenePrefabList.Add(prefab);
            Debug.Log("assigning " + scenePrefabList[i]);
            experimentObj.GetComponent<Experiment>().shopLift.environments[i] = scenePrefabList[i];
            Debug.Log("in shoplift env " + experimentObj.GetComponent<Experiment>().shopLift.environments[i].gameObject.name);
        }

        StartCoroutine("BeginTask");

        //turn off the second environment for now
        scenePrefabList[1].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator BeginTask()
    {
        Debug.Log("about to begin task");
        //yield return StartCoroutine(experimentObj.GetComponent<Experiment>().shopLift.RunTask());
        yield return null;
    }

    GameObject LoadPrefabFromAssetBundle(string bundleName)
    {
        GameObject prefab = null;
        Debug.Log("datapath " + Application.dataPath);
        var path = Path.Combine(Application.dataPath, "AssetBundles/WebGL");
        Debug.Log("path " + path);
#if !UNITY_EDITOR_OSX
        var assetBundlePath = Regex.Split(path,"file://")[1];
#else
        var assetBundlePath = path;
#endif
        Debug.Log("assetbundlepath " + assetBundlePath);
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(assetBundlePath, bundleName));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return prefab;
        }
        prefab = myLoadedAssetBundle.LoadAsset<GameObject>(bundleName);
        Instantiate(prefab);
        return prefab;
    }
}
