using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LibraryDungeonManager : EnvironmentManager
{



    // Use this for initialization
    void Start()
    {
        FillImagesGroupsLD();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Texture LoadTextureLD(string FilePath)
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

    public void FillImagesGroupsLD()
    {
        groupOne[0] = LoadTextureLD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/LibraryDungeon" + "/RoomOne_LD.png");
        groupOne[0].name = "RoomOne_LD";
        groupOne[1] = LoadTextureLD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/LibraryDungeon" + "/RoomTwo_LD.png");
        groupOne[1].name = "RoomTwo_LD";
        groupTwo[0] = LoadTextureLD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/LibraryDungeon" + "/RoomThree_LD.png");
        groupTwo[0].name = "RoomThree_LD";
        groupTwo[1] = LoadTextureLD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/LibraryDungeon" + "/RoomFour_LD.png");
        groupTwo[1].name = "RoomFour_LD";
        groupThree[0] = LoadTextureLD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/LibraryDungeon" + "/RoomFive_LD.png");
        groupThree[0].name = "RoomFive_LD";
        groupThree[1] = LoadTextureLD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/LibraryDungeon" + "/RoomSix_LD.png");
        groupThree[1].name = "RoomSix_LD";
    }
}