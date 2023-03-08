using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

//no reason for this to be a child class
public class ApartmentManager : EnvironmentManager {

	// Use this for initialization
	void Start () {
        FillImagesGroupsAPT();


    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public Texture LoadTextureAPT(string FilePath)
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

    public void FillImagesGroupsAPT()
    {
        groupOne[0] = LoadTextureAPT(Application.dataPath + "/Resources_IGNORE" + "/Themes" +"/Apartment" + "/RoomOne_Apartment.png");
        groupOne[0].name = "RoomOne_Apartment";
        groupOne[1] = LoadTextureAPT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Apartment" + "/RoomTwo_Apartment.png");
        groupOne[1].name = "RoomTwo_Apartment";
        groupTwo[0] = LoadTextureAPT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Apartment" + "/RoomThree_Apartment.png");
        groupTwo[0].name = "RoomThree_Apartment";
        groupTwo[1] = LoadTextureAPT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Apartment" + "/RoomFour_Apartment.png");
        groupTwo[1].name = "RoomFour_Apartment";
        groupThree[0] = LoadTextureAPT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Apartment" + "/RoomFive_Apartment.png");
        groupThree[0].name = "RoomFive_Apartment";
        groupThree[1] = LoadTextureAPT(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Apartment" + "/RoomSix_Apartment.png");
        groupThree[1].name = "RoomSix_Apartment";
    }
}
