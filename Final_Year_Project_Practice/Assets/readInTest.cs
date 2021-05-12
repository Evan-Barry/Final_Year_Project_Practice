using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class readInTest : MonoBehaviour
{
    string path = "Assets/vertices.json";
    public string[] vertexData;
    public int[][] vertexDataInt;
    // Start is called before the first frame update
    void Start()
    {
        StreamReader reader = new StreamReader(path);
        string verticesFromFile = reader.ReadToEnd();
        reader.Close();
        verticesFromFile = verticesFromFile.Substring(1, verticesFromFile.Length-2);
        vertexData = verticesFromFile.Split(']');
        Array.Resize(ref vertexData, vertexData.Length-1);
        vertexDataInt = new int[vertexData.Length][];

        for(int i = 0; i < vertexData.Length; i++)
        {
            vertexData[i] = vertexData[i].Trim(' ',',','[');

            if(i < vertexData.Length-1)
            {
                vertexDataInt[i] = stringToInt(vertexData[i]);
            }
        }
        Debug.Log(vertexDataInt.Length);
    }

    int[] stringToInt(string s)
    {
        string[] split = s.Split(',');
        int[] ints = new int[split.Length];

        for(int i = 0; i < split.Length; i++)
        {
            ints[i] = Int32.Parse(split[i].Trim()[0].ToString());
        }

        return ints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
