using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;

public class DownloadModel : MonoBehaviour {

    private ViewListItem ViewListItem = new ViewListItem();

    public GameObject downloadBtn;
    public GameObject btn_Watch_In_AR;
    public GameObject btn_Update_Download;
    public GameObject menu_Decorate_zone_for_errors;
    public GameObject btn_Cancel_Download;
    public GameObject loader;
    public GameObject btn_Back_To_List;
    public GameLoading _gameLoading;

    public Text textError;

    public string modelID, modelLink, modelDataUpdate, oldModelDataUpdate;

    public string[] modelTexture;
    private string savePath;
    private string savePathDInfo;
    bool downloadOver;
    
    public int currentFileSize;

    // Use this for initialization
    void Start()
    {
        downloadOver = false;
        btn_Back_To_List.SetActive(true);

        modelID = transform.name;

        oldModelDataUpdate = PlayerPrefs.GetString("Model_Update_Date" + modelID);

        string filePath = Application.persistentDataPath + "/data/Json/JsonSettings.json";
        string jsonText = File.ReadAllText(Application.persistentDataPath + "/data/Json/JsonSettings.json");

        ViewListItem = JsonUtility.FromJson<ViewListItem>(jsonText);
        
        savePath = Path.Combine(Application.persistentDataPath, "data");
        savePath = Path.Combine(savePath, "Model" + modelID);
        savePath = Path.Combine(savePath, Path.GetFileName(modelLink));

        savePathDInfo = Path.Combine(Application.persistentDataPath, "data");
        savePathDInfo = Path.Combine(savePathDInfo, "Model" + modelID);


        if (!File.Exists(savePath))
        {
            downloadBtn.SetActive(true);
            ListModelLink();
        }
        else
        {
            downloadBtn.SetActive(false);
            btn_Watch_In_AR.SetActive(true);
        }

        if (oldModelDataUpdate != "" && oldModelDataUpdate != modelDataUpdate && File.Exists(savePath))
        {
            btn_Update_Download.SetActive(true);
            downloadBtn.SetActive(false);
            btn_Watch_In_AR.SetActive(false);
            btn_Update_Download.transform.GetComponent<Button>().onClick.AddListener(BtnPressDownloadModel);
        }

        btn_Watch_In_AR.transform.GetComponent<Button>().onClick.AddListener(WatchInAR);
        btn_Cancel_Download.transform.GetComponent<Button>().onClick.AddListener(CancelDownload);
    }
	// Update is called once per frame
	void Update () {

        if (Directory.Exists(savePathDInfo))
        {
            currentFileSize = Directory.GetFiles(savePathDInfo).Length;
        }
        if (currentFileSize == modelTexture.Length + 1)
        {
            loader.GetComponent<RPB>().speed = 20f;
        }
        if (currentFileSize != modelTexture.Length + 1 && loader.GetComponent<RPB>().currentAmount >= 50)
        {
            loader.GetComponent<RPB>().speed = 1f;
        }
        if (currentFileSize != modelTexture.Length + 1 && loader.GetComponent<RPB>().currentAmount >= 95)
        {
            loader.GetComponent<RPB>().speed = 0.1f;
        }
        if (currentFileSize != modelTexture.Length + 1 && loader.GetComponent<RPB>().currentAmount >= 99)
        {
            loader.GetComponent<RPB>().speed = 0f;
        }
        if (loader.GetComponent<RPB>().currentAmount  >= 100.0f && downloadOver == false && currentFileSize == modelTexture.Length + 1)
        {
            loader.SetActive(false);
            btn_Cancel_Download.SetActive(false);
            btn_Back_To_List.SetActive(true);
            btn_Watch_In_AR.SetActive(true);
            PlayerPrefs.SetString("modelID", modelID);
            DoWork();
        }
    }

    public void CancelDownload()
    {
        loader.SetActive(false);
        loader.GetComponent<RPB>().currentAmount = 0;
        btn_Cancel_Download.SetActive(false);
        StopAllCoroutines();

        string savePathDel = Path.Combine(Application.persistentDataPath, "data");
        savePathDel = Path.Combine(savePathDel, "Model" + modelID);

        Directory.Delete(savePathDel, true);
        Directory.CreateDirectory(savePathDel);

        btn_Back_To_List.SetActive(true);
        downloadBtn.SetActive(true);
    }

    public void DoWork()
    {
        downloadOver = true;
    }
    public void ListModelLink()
    {
        downloadBtn.transform.GetComponent<Button>().onClick.AddListener(BtnPressDownloadModel);
    }

    public void WatchInAR()
    {
        StartCoroutine(WatchInARCor());
    }

    IEnumerator WatchInARCor()
    {
        btn_Back_To_List.SetActive(false);
        btn_Watch_In_AR.GetComponent<Animation>().Play();

        PlayerPrefs.SetString("modelID", modelID);
        string savePathModel = Path.Combine(Application.persistentDataPath, "data");
        savePathModel = Path.Combine(savePathModel, "Model" + modelID);
        savePathModel = Path.Combine(savePathModel, Path.GetFileName(modelLink));
        PlayerPrefs.SetString("modelLink" + modelID, savePathModel);
        PlayerPrefs.SetInt("modelTexture" + modelID, modelTexture.Length);

        //btn_Watch_In_AR.GetComponent<Animation>().Stop();
        //SceneManager.LoadScene("Main");
        _gameLoading.btnMain();
        yield return null;
    }

    public void RetryDownload()
    {
        menu_Decorate_zone_for_errors.SetActive(false);
        BtnPressDownloadModel();
    }

    public void BtnPressDownloadModel()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine(LoaderView());
            //if (currentFileSize != modelTexture.Length + 1)
            //{
            //    menu_Decorate_zone_for_errors.SetActive(true);
            //    textError.text = "The model is not found";
            //}
            //else
            //{
            //    StartCoroutine(loaderView());
            //}
        }
        else
        {
            menu_Decorate_zone_for_errors.SetActive(true);
            textError.text = "Check your internet connection";
        }

    }

    public void OffGameObject()
    {
        downloadBtn.SetActive(false);
        btn_Update_Download.SetActive(false);
        btn_Back_To_List.SetActive(false);
    }

    IEnumerator LoaderView()
    {
        OffGameObject();

        if (oldModelDataUpdate != "" && oldModelDataUpdate != modelDataUpdate)
        {
            string savePathDel = Path.Combine(Application.persistentDataPath, "data");
            savePathDel = Path.Combine(savePathDel, "Model" + modelID);

            Directory.Delete(savePathDel, true);
            Directory.CreateDirectory(savePathDel);
        }

        btn_Cancel_Download.SetActive(true);

        loader.SetActive(true);
        string savePathModel = Path.Combine(Application.persistentDataPath, "data");
        savePathModel = Path.Combine(savePathModel, "Model" + modelID);
        savePathModel = Path.Combine(savePathModel, Path.GetFileName(modelLink));
        
        DownloadFile(modelLink, savePathModel);

        PlayerPrefs.SetString("modelLink" + modelID, savePathModel);

        PlayerPrefs.SetString("Model_Update_Date" + modelID, modelDataUpdate);

        for (int i = 1; i <= modelTexture.Length; i++)
        {
            string savePathTexture = Path.Combine(Application.persistentDataPath, "data");
            savePathTexture = Path.Combine(savePathTexture, "Model" + modelID);
            savePathTexture = Path.Combine(savePathTexture, Path.GetFileName(modelTexture[i - 1]));

            DownloadFile(modelTexture[i - 1], savePathTexture);
        }
        PlayerPrefs.SetInt("modelTexture" + modelID, modelTexture.Length);

        yield return new WaitForSeconds(1.0f);
        
    }

    public void DownloadFile(string url, string pathToSaveModel)
    {
        WWW www = new WWW(url);
        StartCoroutine(_downloadFile(www, pathToSaveModel));
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
