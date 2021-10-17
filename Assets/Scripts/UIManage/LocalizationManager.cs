using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;
    private Dictionary<string, string> localizedText;
    private string missingTextString = "Localized text not found";

    void Awake()
    {

        // singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // DontDestroyOnLoad(gameObject);
    }

    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        filePath += ".txt";
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath); // deserialize JSON into string type
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson); // deserialization

            // for all items
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            // load LocalizedText
            Debug.Log("Data loaded. Dictionary containts :" + localizedText.Count + " entries");
        }
        else
        {
            // if there's no file
            Debug.LogError("Cannot find file");
        }

    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;
    }
}