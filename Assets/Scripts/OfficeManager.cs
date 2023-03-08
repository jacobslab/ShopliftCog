using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class OfficeManager : EnvironmentManager {

	// Use this for initialization
	void Start () {
        FillImagesGroupsWT();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Texture LoadTextureOFF(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;
        Debug.Log("LoadTexture: FilePath: " + FilePath);
        if (File.Exists(FilePath))
        {
            Debug.Log("LoadTexture: FilePath: Exists: " + FilePath);
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

    public void FillImagesGroupsWT()
    {
        groupOne[0] = LoadTextureOFF(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Office" + "/RoomOne_Office.png");
        groupOne[0].name = "RoomOne_Office";
        groupOne[1] = LoadTextureOFF(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Office" + "/RoomTwo_Office.png");
        groupOne[1].name = "RoomTwo_Office";
        groupTwo[0] = LoadTextureOFF(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Office" + "/RoomThree_Office.png");
        groupTwo[0].name = "RoomThree_Office";
        groupTwo[1] = LoadTextureOFF(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Office" + "/RoomFour_Office.png");
        groupTwo[1].name = "RoomFour_Office";
        groupThree[0] = LoadTextureOFF(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Office" + "/RoomFive_Office.png");
        groupThree[0].name = "RoomFive_Office";
        groupThree[1] = LoadTextureOFF(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Office" + "/RoomSix_Office.png");
        groupThree[1].name = "RoomSix_Office";
    }

}
