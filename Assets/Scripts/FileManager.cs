using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private string filePath;

    private static string DEMO_DATA = "1:0|2:0|3:0|";
    void OnEnable()
    {
        string projectPath = AppDomain.CurrentDomain.BaseDirectory;

        filePath = Application.persistentDataPath + "/data.txt";

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            WriteToFile(DEMO_DATA);
        }

        Debug.Log(filePath);
    }

    public Dictionary<string, string> loadSaveData()
    {
        Dictionary<string, string> saveData = new Dictionary<string, string>();

        string rawData = ReadFromFile();
        string[] dataLine = rawData.Split("|");

        foreach (string item in dataLine)
        {
            if (item.Length > 0 )
            {
                string[] temp = item.Split(":");
                saveData.Add(temp[0], temp[1]);

                Debug.Log(temp[0] + " " + temp[1]);
            }
        }

        return saveData;
    }

    public void saveSaveData(Dictionary<string, string> data)
    {
        string saveData = "";
        foreach (KeyValuePair<string, string> pair in data)
        {
            string key = pair.Key;
            string value = pair.Value;

            saveData += key + ":" + value + "|";
        }

        File.WriteAllText(filePath, saveData);
    }

    private void WriteToFile(string data)
    {
        StreamWriter writer = new StreamWriter(filePath, false);

        writer.WriteLine(data);

        writer.Close();
    }

    private string ReadFromFile()
    {
        string data = "";

        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);

            data = reader.ReadLine();

            reader.Close();
        }

        return data;
    }
}
