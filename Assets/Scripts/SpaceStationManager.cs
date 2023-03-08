using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpaceStationManager : EnvironmentManager {



	// Use this for initialization
	void Start () {
        FillImagesGroupsSS();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Texture LoadTextureSS(string FilePath)
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

    public void FillImagesGroupsSS()
    {
        groupOne[0] = LoadTextureSS(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/SpaceStation" + "/RoomOne_S.png");
        groupOne[0].name = "RoomOne_S";
        groupOne[1] = LoadTextureSS(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/SpaceStation" + "/RoomTwo_S.png");
        groupOne[1].name = "RoomTwo_S";
        groupTwo[0] = LoadTextureSS(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/SpaceStation" + "/RoomThree_S.png");
        groupTwo[0].name = "RoomThree_S";
        groupTwo[1] = LoadTextureSS(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/SpaceStation" + "/RoomFour_S.png");
        groupTwo[1].name = "RoomFour_S";
        groupThree[0] = LoadTextureSS(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/SpaceStation" + "/RoomFive_S.png");
        groupThree[0].name = "RoomFive_S";
        groupThree[1] = LoadTextureSS(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/SpaceStation" + "/RoomSix_S.png");
        groupThree[1].name = "RoomSix_S";
    }
}
