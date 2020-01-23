using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using TMPro;

public class ReaderFile : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public TextAsset file;
    public GameObject prefabCube;
    public ColorCube cubeColor;

    [Range(0f,1f)]
    public float redColor = 0;
    [Range(0f,1f)]
    public float greenColor = 0;
    [Range(0f,1f)]
    public float blueColor = 0;


    byte[] byteSave;
    string path = "/Controler.json";
    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath+path;
        displayText.text = path;
        var sr = File.CreateText(path);
        sr.Write(JsonUtility.ToJson(cubeColor));
        // File.WriteAllText(path, JsonUtility.ToJson(cubeColor));
        sr.Close();
        byteSave = File.ReadAllBytes(path);
    }
    void Update(){
        ReadBytes();
        WriteBytes();
    }

    public void ReadBytes(){
        if(byteSave != File.ReadAllBytes(path)){
            try{
                using (FileStream fs = File.OpenRead(path)){
                    string jsonColor = File.ReadAllText(path);
                    ChangeCube(JsonUtility.FromJson<ColorCube>(jsonColor));
                }
            }
            catch(IOException)
            {
                Debug.LogWarning("Wait few second to read");
            }
        }
    }
    public void WriteBytes(){
        if(cubeColor.colorCube.r != redColor || cubeColor.colorCube.g != greenColor || cubeColor.colorCube.b != blueColor){
            try{
                using (StreamWriter fs = File.CreateText(path)){
                    cubeColor.colorCube = new Color(redColor,greenColor,blueColor);
                    fs.Write(JsonUtility.ToJson(cubeColor));
                    // File.WriteAllText(path, JsonUtility.ToJson(cubeColor));
                    fs.Close();
                }
            }
            catch(IOException)
            {
                Debug.LogWarning("Wait few second to write");
            }
        }
    }

    public void ChangeCube(ColorCube cube){
        byteSave = File.ReadAllBytes(path);
        cubeColor = cube;
        prefabCube.GetComponent<MeshRenderer>().material.color = cubeColor.colorCube;
        redColor = cubeColor.colorCube.r;
        greenColor = cubeColor.colorCube.g;
        blueColor = cubeColor.colorCube.b;
    }
}

[Serializable]
public class ColorCube{
    public Color colorCube;
}
