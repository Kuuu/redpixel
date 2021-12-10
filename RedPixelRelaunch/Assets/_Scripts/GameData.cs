using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameData : MonoBehaviour
{
    public static GameData Instance;
    private MyDict myDict;
    private Dictionary<String, int> gameData;

    private TimeDict timeDict;
    private Dictionary<String, float> timeData;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Set(String key, int value)
    {
        LoadData();
        if (gameData.ContainsKey(key))
        {
            gameData[key] = value;
        } else
        {
            gameData.Add(key, value);
        }
        SaveData();
    }

    public int Get(String key)
    {
        LoadData();
        int value = -1;
        if (gameData.ContainsKey(key))
        {
            gameData.TryGetValue(key, out value);
        }
        
        return value;
    }

    public void SetTime(String key, float value)
    {
        LoadTimeData();
        if (timeData.ContainsKey(key))
        {
            timeData[key] = value;
        }
        else
        {
            timeData.Add(key, value);
        }
        SaveTimeData();
    }

    public float GetTime(String key)
    {
        LoadTimeData();
        float value = 0;
        if (timeData.ContainsKey(key))
        {
            timeData.TryGetValue(key, out value);
        }

        return value;
    }


    public void SaveData()
    {
        myDict._keys = new List<String>(gameData.Keys);
        myDict._values = new List<int>(gameData.Values);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
                     + "/MySaveData.dat");
        bf.Serialize(file, myDict);
        file.Close();
        //Debug.Log("Game data saved!");
    }

    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath
                       + "/MySaveData.dat", FileMode.Open);
            MyDict loadedData = (MyDict)bf.Deserialize(file);
            file.Close();
            myDict = loadedData;
            //Debug.Log("Game data loaded!");
            gameData = Enumerable.Range(0, myDict._keys.Count).ToDictionary(i => myDict._keys[i], i => myDict._values[i]);
        }
        else
        {
            myDict = new MyDict();
            gameData = new Dictionary<string, int>();
            //Debug.Log("No data found. Creating new Dict.");
        }
    }

    public void SaveTimeData()
    {
        timeDict._keys = new List<String>(timeData.Keys);
        timeDict._values = new List<float>(timeData.Values);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
                     + "/MyTimeData.dat");
        bf.Serialize(file, timeDict);
        file.Close();
        //Debug.Log("Time data saved!");
    }

    public void LoadTimeData()
    {
        //Debug.Log(Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/MyTimeData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath
                       + "/MyTimeData.dat", FileMode.Open);
            TimeDict loadedData = (TimeDict)bf.Deserialize(file);
            file.Close();
            timeDict = loadedData;
            //Debug.Log("Time data loaded!");
            timeData = Enumerable.Range(0, timeDict._keys.Count).ToDictionary(i => timeDict._keys[i], i => timeDict._values[i]);
        }
        else
        {
            timeDict = new TimeDict();
            timeData = new Dictionary<string, float>();
            //Debug.Log("No time data found. Creating new Dict.");
        }
    }

    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/MySaveData.dat");
            //Debug.Log("Data reset complete!");
        }
        else
        {
            //Debug.Log("No save data to delete.");
        }

        if (File.Exists(Application.persistentDataPath + "/MyTimeData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/MyTimeData.dat");
            //Debug.Log("Time Data reset complete!");
        }
        else
        {
            //Debug.Log("No time data to delete.");
        }
    }
}

[Serializable]
public class MyDict
{
    public List<String> _keys = new List<String>();
    public List<int> _values = new List<int>();
}

[Serializable]
public class TimeDict
{
    public List<String> _keys = new List<String>();
    public List<float> _values = new List<float>();
}