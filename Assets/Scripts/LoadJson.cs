using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;

public class LoadJson : MonoBehaviour
{
    private ViewListItem ViewListItem = new ViewListItem();
    private ViewListItem ViewListItemTemp = new ViewListItem();
    private string jsonfile = "http://api.plyserver.com/list/?key=df818b1ee44399e618350d09443d05c7";

    public string sceneName;

    public GameObject loader;
    private string savePathJson;
    private string savePathJsonNew;
    private bool download;

    public GameObject noConection;

    private string file_Update_Date;

    public GameObject cancelL;

    public int currentFileSize;
    public int currentFileInData;
    string jsonDir, bgDir, imgDir, dataDir;
    string jsonTextInternet;
    public Text textInternet;

    void Start()
    {
        file_Update_Date = PlayerPrefs.GetString("file_Update_Date");

        PlayerPrefs.SetString("removeModelID", "");
        download = false;

        jsonDir = Path.Combine(Application.persistentDataPath, "data");
        jsonDir = Path.Combine(jsonDir, "Json");

        bgDir = Path.Combine(Application.persistentDataPath, "data");
        bgDir = Path.Combine(bgDir, "Backgrounds");

        imgDir = Path.Combine(Application.persistentDataPath, "data");
        imgDir = Path.Combine(imgDir, "Images");

        dataDir = Path.Combine(Application.persistentDataPath, "data");

        savePathJson = Path.Combine(Application.persistentDataPath, "data");
        savePathJson = Path.Combine(savePathJson, "Json");
        savePathJson = Path.Combine(savePathJson, "JsonSettings" + ".json");

        savePathJsonNew = Path.Combine(Application.persistentDataPath, "data");
        savePathJsonNew = Path.Combine(savePathJsonNew, "Json");
        savePathJsonNew = Path.Combine(savePathJsonNew, "JsonSettingsTemp" + ".json");

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (File.Exists(savePathJson))
            {
                string jsonText = File.ReadAllText(Application.persistentDataPath + "/data/Json/JsonSettings.json");
                ViewListItem = JsonUtility.FromJson<ViewListItem>(jsonText);

                if (Directory.Exists(jsonDir) && Directory.Exists(bgDir) && Directory.Exists(imgDir))
                {
                    currentFileSize = Directory.GetFiles(jsonDir).Length + Directory.GetFiles(bgDir).Length + Directory.GetFiles(imgDir).Length;
                }

                if (currentFileSize == (ViewListItem.Items_Count * 2) + 2)
                {
                    DownloadFile(jsonfile, savePathJsonNew);
                    StartCoroutine(ChackJsonTemp());
                }
                else
                {
                    if (Directory.Exists(jsonDir))
                    {
                        Directory.Delete(jsonDir, true);
                    }
                    if (Directory.Exists(bgDir))
                    {
                        Directory.Delete(bgDir, true);
                    }
                    if (Directory.Exists(imgDir))
                    {
                        Directory.Delete(imgDir, true);
                    }
                    DownloadFile(jsonfile, savePathJson);
                    download = true;
                }
            }
            else
            {
                if (Directory.Exists(jsonDir))
                {
                    Directory.Delete(jsonDir, true);
                }
                if (Directory.Exists(bgDir))
                {
                    Directory.Delete(bgDir, true);
                }
                if (Directory.Exists(imgDir))
                {
                    Directory.Delete(imgDir, true);
                }
                DownloadFile(jsonfile, savePathJson);
                download = true;
            }


            //DownloadFile(jsonfile, savePathJsonNew);
            //DownloadFile(jsonfile, savePathJson);
            //download = true;
            noConection.SetActive(false);
            //if (!File.Exists(savePathJson))
            //{
            //    DownloadFile(jsonfile, savePathJson);
            //    download = true;
            //}
            //else
            //{
            //    StartCoroutine(ChackJsonTemp());
            // }
        }
        else
        {
            loader.SetActive(false);
            loader.GetComponent<RPB>().currentAmount = 0f;
            noConection.SetActive(true);
            textInternet.text = "Check your internet connection";
        }
        // else if (HtmlText == "" && !File.Exists(Application.persistentDataPath + "/data/Json/JsonSettings.json"))
        // {
        //     loader.SetActive(false);
        //     loader.GetComponent<RPB>().currentAmount = 0f;
        //     noConection.SetActive(true);
        //     textInternet.text = "JSON-file is not found";
        // }

    }

    IEnumerator ChackJsonTemp()
    {
        yield return new WaitForSeconds(3.0f);

            string jsonText = File.ReadAllText(Application.persistentDataPath + "/data/Json/JsonSettings.json");
            string jsonTextNew = File.ReadAllText(Application.persistentDataPath + "/data/Json/JsonSettingsTemp.json");
            ViewListItem = JsonUtility.FromJson<ViewListItem>(jsonText);
            ViewListItemTemp = JsonUtility.FromJson<ViewListItem>(jsonTextNew);

            if (ViewListItem.File_Update_Date != ViewListItemTemp.File_Update_Date)
            {

                if (Directory.Exists(jsonDir))
                {
                    Directory.Delete(jsonDir, true);
                }
                if (Directory.Exists(bgDir))
                {
                    Directory.Delete(bgDir, true);
                }
                if (Directory.Exists(imgDir))
                {
                    Directory.Delete(imgDir, true);
                }
                DownloadFile(jsonfile, savePathJson);
                StartCoroutine(LoaderView());
            }
            else
            {
                File.Delete(savePathJsonNew);
                loader.SetActive(true);
                loader.GetComponent<RPB>().maxAmout = 100.0f;
            }
        

    }

    public void CancelGoToJson()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable && File.Exists(Application.persistentDataPath + "/data/Json/JsonSettings.json"))
        {
            StartCoroutine(LoadNextScene());
        }
        else if (Application.internetReachability == NetworkReachability.NotReachable && !File.Exists(Application.persistentDataPath + "/data/Json/JsonSettings.json"))
        {
            Exit();
        }

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void RetryDownload()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            //DownloadFile(jsonfile, savePathJsonNew);
            DownloadFile(jsonfile, savePathJson);
            download = true;
            noConection.SetActive(false);
            //if (!File.Exists(savePathJson))
            //{
            //    DownloadFile(jsonfile, savePathJson);
            //    download = true;
            //}
            //else
            //{
            //    StartCoroutine(ChackJsonTemp());
            // }
        }
        else
        {
            loader.SetActive(false);
            loader.GetComponent<RPB>().currentAmount = 0f;
            noConection.SetActive(true);
            textInternet.text = "Check your internet connection";
        }
    }

    public void PreListOfObjects()
    {
        string filePath = Application.persistentDataPath + "/data/Json/JsonSettings.json";
        string jsonText = File.ReadAllText(Application.persistentDataPath + "/data/Json/JsonSettings.json");
        ViewListItem = JsonUtility.FromJson<ViewListItem>(jsonText);
        PlayerPrefs.SetString("file_Update_Date", ViewListItem.File_Update_Date);

        for (int i = 1; i <= ViewListItem.Items_Count; i++)
        {
            string savePath = Path.Combine(Application.persistentDataPath, "data");
            savePath = Path.Combine(savePath, "Images");
            savePath = Path.Combine(savePath, ViewListItem.List[i - 1].Item_id);

            string savePathBG = Path.Combine(Application.persistentDataPath, "data");
            savePathBG = Path.Combine(savePathBG, "Backgrounds");
            savePathBG = Path.Combine(savePathBG, ViewListItem.List[i - 1].Item_id + "BG");

            DownloadImage(ViewListItem.List[i - 1].Thumbnail, savePath);
            DownloadImage(ViewListItem.List[i - 1].Bg_Image, savePathBG);
        }

        string savePathBGList = Path.Combine(Application.persistentDataPath, "data");
        savePathBGList = Path.Combine(savePathBGList, "Backgrounds");
        savePathBGList = Path.Combine(savePathBGList, "BGList");

        DownloadImage(ViewListItem.Bg_List, savePathBGList);
    }

    void Update()
    {
        //if (Directory.Exists(dataDir))
        //{
        //    currentFileInData = Directory.GetDirectories(dataDir).Length;
        //}
        //if (currentFileInData >= 3 )
        if (Directory.Exists(jsonDir) && Directory.Exists(bgDir) && Directory.Exists(imgDir))
        {
            currentFileSize = Directory.GetFiles(jsonDir).Length + Directory.GetFiles(bgDir).Length + Directory.GetFiles(imgDir).Length;
        }
        if (currentFileSize == (ViewListItem.Items_Count * 2) + 2)
        {
            loader.GetComponent<RPB>().speed = 20f;
        }
        if (currentFileSize != (ViewListItem.Items_Count * 2) + 2 && loader.GetComponent<RPB>().currentAmount >= 50)
        {
            loader.GetComponent<RPB>().speed = 1f;
        }
        if (currentFileSize != (ViewListItem.Items_Count * 2) + 2 && loader.GetComponent<RPB>().currentAmount >= 95)
        {
            loader.GetComponent<RPB>().speed = 0.1f;
        }
        if (currentFileSize != (ViewListItem.Items_Count * 2) + 2 && loader.GetComponent<RPB>().currentAmount >= 99)
        {
            loader.GetComponent<RPB>().speed = 0f;
        }
        if (File.Exists(savePathJson) && download == true)
        {
            StartCoroutine(LoaderView());
            download = false;
        }
        if (loader.GetComponent<RPB>().currentAmount >= 100.0f && currentFileSize == (ViewListItem.Items_Count * 2) + 2)
        {
            File.Delete(savePathJsonNew);
            StartCoroutine(LoadNextScene());
        }
    }


    IEnumerator LoaderView()
    {
        loader.SetActive(true);
        loader.GetComponent<RPB>().maxAmout = 100.0f;

        //yield return new WaitForSeconds(3.0f);

        PreListOfObjects();

        yield return new WaitForSeconds(1.0f);

    }
    public IEnumerator LoadNextScene()
    {
        SceneManager.LoadScene(sceneName);
        yield return null;
    }

    public void DownloadImage(string url, string pathToSaveImage)
    {
        WWW www = new WWW(url);
        StartCoroutine(_downloadImage(www, pathToSaveImage));
    }

    public void DownloadFile(string url, string pathToSaveModel)
    {
        WWW www = new WWW(url);
        StartCoroutine(_downloadFile(www, pathToSaveModel));
    }


    private IEnumerator _downloadImage(WWW www, string savePath)
    {
        yield return www;

        //Check if we failed to send
        if (string.IsNullOrEmpty(www.error))
        {
            UnityEngine.Debug.Log("Success");

            //Save Image
            SaveImage(savePath, www.bytes);

        }
        else
        {
            UnityEngine.Debug.Log("Error: " + www.error);
        }
    }

    private IEnumerator _downloadFile(WWW www, string savePath)
    {
        yield return www;

        //Check if we failed to send
        if (string.IsNullOrEmpty(www.error))
        {
            UnityEngine.Debug.Log("Success");

            //Save Image
            SaveFile(savePath, www.bytes);

        }
        else
        {
            UnityEngine.Debug.Log("Error: " + www.error);
        }
    }

    void SaveImage(string path, byte[] imageBytes)
    {
        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, imageBytes);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    void SaveFile(string path, byte[] modelBytes)
    {
        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, modelBytes);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    byte[] LoadImage(string path)
    {
        byte[] dataByte = null;

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Debug.LogWarning("Directory does not exist");
            return null;
        }

        if (!File.Exists(path))
        {
            Debug.Log("File does not exist");
            return null;
        }

        try
        {
            dataByte = File.ReadAllBytes(path);
            Debug.Log("Loaded Data from: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        return dataByte;
    }

    byte[] LoadFile(string path)
    {
        byte[] dataByte = null;

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Debug.LogWarning("Directory does not exist");
            return null;
        }

        if (!File.Exists(path))
        {
            Debug.Log("File does not exist");
            return null;
        }

        try
        {
            dataByte = File.ReadAllBytes(path);
            Debug.Log("Loaded Data from: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        return dataByte;
    }

}