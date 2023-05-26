using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MedievalDungeonManager : EnvironmentManager
{



    // Use this for initialization
    void Start()
    {
        FillImagesGroupsMD();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Texture LoadTextureMD(string FilePath)
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

    public void FillImagesGroupsMD()
    {
        groupOne[0] = LoadTextureMD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/MedievalDungeon" + "/RoomOne_MD.png");
        groupOne[0].name = "RoomOne_MD";
        groupOne[1] = LoadTextureMD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/MedievalDungeon" + "/RoomTwo_MD.png");
        groupOne[1].name = "RoomTwo_MD";
        groupTwo[0] = LoadTextureMD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/MedievalDungeon" + "/RoomThree_MD.png");
        groupTwo[0].name = "RoomThree_MD";
        groupTwo[1] = LoadTextureMD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/MedievalDungeon" + "/RoomFour_MD.png");
        groupTwo[1].name = "RoomFour_MD";
        groupThree[0] = LoadTextureMD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/MedievalDungeon" + "/RoomFive_MD.png");
        groupThree[0].name = "RoomFive_MD";
        groupThree[1] = LoadTextureMD(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/MedievalDungeon" + "/RoomSix_MD.png");
        groupThree[1].name = "RoomSix_MD";
    }
}