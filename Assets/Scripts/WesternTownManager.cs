using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WesternTownManager : EnvironmentManager {

	// Use this for initialization
	void Start () {
        FillImagesGroupsWT();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public Texture LoadTextureWT(string FilePath)
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
        groupOne[0] = LoadTextureWT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/WesternTown" + "/ROne_WT.png");
        groupOne[0].name = "ROne_WT";
        groupOne[1] = LoadTextureWT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/WesternTown" + "/RTwo_WT.png");
        groupOne[1].name = "RTwo_WT";
        groupTwo[0] = LoadTextureWT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/WesternTown" + "/RThree_WT.png");
        groupTwo[0].name = "RThree_WT";
        groupTwo[1] = LoadTextureWT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/WesternTown" + "/RFour_WT.png");
        groupTwo[1].name = "RFour_WT";
        groupThree[0] = LoadTextureWT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/WesternTown" + "/RFive_WT.png");
        groupThree[0].name = "RFive_WT";
        groupThree[1] = LoadTextureWT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/WesternTown" + "/RSix_WT.png");
        groupThree[1].name = "RSix_WT";
    }
}
