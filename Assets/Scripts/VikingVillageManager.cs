using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VikingVillageManager : EnvironmentManager {

	// Use this for initialization
	void Start () {
        FillImagesGroupsVV();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Texture LoadTextureVV(string FilePath)
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

    public void FillImagesGroupsVV()
    {
        groupOne[0] = LoadTextureVV(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/VikingVillage" + "/RoomOne_Viking.png");
        groupOne[0].name = "RoomOne_Viking";
        groupOne[1] = LoadTextureVV(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/VikingVillage" + "/RoomTwo_Viking.png");
        groupOne[1].name = "RoomTwo_Viking";
        groupTwo[0] = LoadTextureVV(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/VikingVillage" + "/RoomThree_Viking.png");
        groupTwo[0].name = "RoomThree_Viking";
        groupTwo[1] = LoadTextureVV(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/VikingVillage" + "/RoomFour_Viking.png");
        groupTwo[1].name = "RoomFour_Viking";
        groupThree[0] = LoadTextureVV(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/VikingVillage" + "/RoomFive_Viking.png");
        groupThree[0].name = "RoomFive_Viking";
        groupThree[1] = LoadTextureVV(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/VikingVillage" + "/RoomSix_Viking.png");
        groupThree[1].name = "RoomSix_Viking";
    }

}
