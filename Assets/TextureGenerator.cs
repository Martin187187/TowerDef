using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;
    public Color backgroundColor = Color.white;
    public Color pixelColor = Color.red;

    public CubeMovementOnTexture cube;
    private Texture2D texture;

    public int counter = 0;

    void Start()
    {
        GenerateTexture();
    }

    void Update()
    {
        Vector2Int currentPos = new Vector2Int(
            (int)((cube.transform.position.x + cube.borderSize)/(cube.borderSize*2)*(textureWidth-1)), 
            (int)((cube.transform.position.y + cube.borderSize)/(cube.borderSize*2)*(textureHeight-1))
            );
            int repeatR = 256;
            int repeatB = repeatR*repeatR;
            int repeatG = repeatR*repeatR*repeatR;
            float red =(counter%(float)repeatR)/repeatR;
            float blue =(counter%(float)repeatB)/repeatB;
            float green =(counter%(float)repeatG)/repeatG;

            texture.SetPixel(currentPos.x, currentPos.y, new Color(red, blue, green));
            texture.Apply();
            counter = (counter + 1) % repeatG;

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveTexture();
        }
    }
    void SaveTexture()
    {
        // Convert the Texture2D to a PNG byte array
        byte[] bytes = texture.EncodeToPNG();

        // Specify the file path (you may want to customize this)
        string filePath = Application.dataPath + "/SavedTexture.png";

        // Write the PNG byte array to a file
        System.IO.File.WriteAllBytes(filePath, bytes);

        Debug.Log("Texture saved to: " + filePath);
    }


    void GenerateTexture()
    {
        // Create a new Texture2D
        texture = new Texture2D(textureWidth, textureHeight);

        // Set background color
        Color[] backgroundColorArray = new Color[textureWidth * textureHeight];
        for (int i = 0; i < backgroundColorArray.Length; i++)
        {
            backgroundColorArray[i] = backgroundColor;
        }
        texture.SetPixels(backgroundColorArray);

        // Set pixel color
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                texture.SetPixel(x, y, backgroundColor);
            }
        }

        // Apply changes
        texture.Apply();

        // Assign the texture to a material or use it as needed
        GetComponent<MeshRenderer>().material.mainTexture = texture;
    }
}