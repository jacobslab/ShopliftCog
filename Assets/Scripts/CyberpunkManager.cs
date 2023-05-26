using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CyberpunkManager : EnvironmentManager
{



    // Use this for initialization
    void Start()
    {
        FillImagesGroupsCP();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Texture LoadTextureCP(string FilePath)
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

    public void FillImagesGroupsCP()
    {
        groupOne[0] = LoadTextureCP(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Cyberpunk" + "/RoomOne_CP.png");
        groupOne[0].name = "RoomOne_CP";
        groupOne[1] = LoadTextureCP(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Cyberpunk" + "/RoomTwo_CP.png");
        groupOne[1].name = "RoomTwo_CP";
        groupTwo[0] = LoadTextureCP(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Cyberpunk" + "/RoomThree_CP.png");
        groupTwo[0].name = "RoomThree_CP";
        groupTwo[1] = LoadTextureCP(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Cyberpunk" + "/RoomFour_CP.png");
        groupTwo[1].name = "RoomFour_CP";
        groupThree[0] = LoadTextureCP(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Cyberpunk" + "/RoomFive_CP.png");
        groupThree[0].name = "RoomFive_CP";
        groupThree[1] = LoadTextureCP(Application.dataPath + "/Resources_IGNORE" + "/Themes" + "/Cyberpunk" + "/RoomSix_CP.png");
        groupThree[1].name = "RoomSix_CP";
    }
}