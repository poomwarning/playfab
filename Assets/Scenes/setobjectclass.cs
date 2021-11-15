using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class setobjectclass : MonoBehaviour
{
    public static string directory = "/SaveData/";
    public static string fileName = "MyData.txt";
    public Progress oof;
  
    public static void Save (Progress so)
    {
        string dir = Application.persistentDataPath + directory;

        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        //string Json = JsonUtility.ToJson(so);
         string Json = JsonConvert.SerializeObject(so);
        File.WriteAllText(dir + fileName,Json);

    }
    public static Progress load()
    {
        string fullPath = Application.persistentDataPath + directory+fileName;
        Progress so = new Progress();
        if(File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            //so = JsonUtility.FromJson<Progress>(json);
            so = JsonConvert.DeserializeObject<Progress>(json);
        }
        else
        {
            Debug.Log("save file does not exist");
        }
        return so;
    }
    public void saveJSONtoPC()
    {
        Save(oof);
    }
    public void loadJsonfromPC()
    {
       oof = load();
    }
   
}
