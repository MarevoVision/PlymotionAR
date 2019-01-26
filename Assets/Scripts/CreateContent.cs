using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateContent : MonoBehaviour {

    public ViewListItem ViewListItem = new ViewListItem();
    public GameObject prebafObj;
    public GameObject prebafDM;
    public GameObject contentButtons;
    public GameObject canvas;
    public Image bg_list_img;

    public GameObject scrollObjects;
    // private Image logo_model;
    Texture2D tex;
    
    string url;
    public int curentCount;

    string nameBtnModel;
    private string backPath;
    private string backPath2;
    private string remModelID;

    string itemID;


    // Use this for initialization
    void Start () {
        string filePath = Application.persistentDataPath + "/data/Json/JsonSettings.json";
        string jsonText = File.ReadAllText(Application.persistentDataPath + "/data/Json/JsonSettings.json");

        ViewListItem = JsonUtility.FromJson<ViewListItem>(jsonText);

        remModelID = PlayerPrefs.GetString("removeModelID");
        

        for (int i = 1; i <= ViewListItem.Items_Count; i++)
        {
            GameObject buttonModel = Instantiate(prebafObj, contentButtons.transform) as GameObject;
            buttonModel.name = ViewListItem.List[i - 1].Item_id;
            buttonModel.transform.SetParent(contentButtons.transform);
            
            buttonModel.transform.Find("Description/DescriptionMode/Description_Title").GetComponent<Text>().text = ViewListItem.List[i - 1].Title;
            buttonModel.transform.Find("Description/DescriptionMode/Description_Text").GetComponent<Text>().text = ViewListItem.List[i - 1].Short_Description;
            
            GameObject downloadsMenu = Instantiate(prebafDM, canvas.transform) as GameObject;
            downloadsMenu.name = ViewListItem.List[i - 1].Item_id;
            downloadsMenu.transform.SetParent(canvas.transform);

            downloadsMenu.GetComponent<DownloadModel>().modelID = ViewListItem.List[i - 1].Item_id;
            downloadsMenu.GetComponent<DownloadModel>().modelLink = ViewListItem.List[i - 1].Model_Link;
            downloadsMenu.GetComponent<DownloadModel>().modelTexture = ViewListItem.List[i - 1].TextureArray;
            downloadsMenu.GetComponent<DownloadModel>().modelDataUpdate = ViewListItem.List[i - 1].Model_Update_Date;

            downloadsMenu.transform.Find("dwn/VerticalMoveText/Content/Description_Title").GetComponent<Text>().text = ViewListItem.List[i - 1].Title;
            downloadsMenu.transform.Find("dwn/VerticalMoveText/Content/Description_bg/Description/Content/Description_Text").GetComponent<Text>().text = ViewListItem.List[i - 1].Full_Description;

            backPath = ViewListItem.List[i - 1].Thumbnail;
            
            string savePathBGList = Path.Combine(Application.persistentDataPath, "data");
            savePathBGList = Path.Combine(savePathBGList, "Backgrounds");
            savePathBGList = Path.Combine(savePathBGList, "BGList");

            byte[] imageBytesBGList = loadImage(savePathBGList);

            Texture2D textureBGList;
            textureBGList = new Texture2D(2, 2);
            textureBGList.LoadImage(imageBytesBGList);

            bg_list_img.sprite = Sprite.Create(textureBGList, new Rect(0.0f, 0.0f, textureBGList.width, textureBGList.height), new Vector2(0.5f, 0.5f), 100.0f);

            string savePath = Path.Combine(Application.persistentDataPath, "data");
            savePath = Path.Combine(savePath, "Images");
            savePath = Path.Combine(savePath, ViewListItem.List[i - 1].Item_id);

            byte[] imageBytes = loadImage(savePath);

            Texture2D texture;
            texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            
            buttonModel.transform.Find("Description/Logo_Model").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            string savePathBG = Path.Combine(Application.persistentDataPath, "data");
            savePathBG = Path.Combine(savePathBG, "Backgrounds");
            savePathBG = Path.Combine(savePathBG, ViewListItem.List[i - 1].Item_id + "BG");

            byte[] imageBytesBG = loadImage(savePathBG);

            Texture2D textureBG;
            textureBG = new Texture2D(2, 2);
            textureBG.LoadImage(imageBytesBG);

            downloadsMenu.transform.Find("dwn/BG").GetComponent<Image>().sprite = Sprite.Create(textureBG, new Rect(0.0f, 0.0f, textureBG.width, textureBG.height), new Vector2(0.5f, 0.5f), 100.0f);

            //itemID = ViewListItem.List[i - 1].Item_id;

            //buttonModel.transform.GetComponent<Button>().onClick.AddListener(PressButton);

            //buttonModel.transform.Find(itemID).GetComponent<Button>().onClick.AddListener(PressButton);
            
        }

        if (remModelID != "")
        {
           canvas.transform.Find(remModelID + "/dwn").gameObject.SetActive(true);
        }
    }
  
    public void downloadImage(string url, string pathToSaveImage)
    {
        WWW www = new WWW(url);
        StartCoroutine(_downloadImage(www, pathToSaveImage));
    }

    private IEnumerator _downloadImage(WWW www, string savePath)
    {
        yield return www;

        //Check if we failed to send
        if (string.IsNullOrEmpty(www.error))
        {
           // UnityEngine.Debug.Log("Success");

            //Save Image
            saveImage(savePath, www.bytes);
            
        }
        else
        {
          //  UnityEngine.Debug.Log("Error: " + www.error);
        }
    }

    void saveImage(string path, byte[] imageBytes)
    {
        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, imageBytes);
           // Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
           // Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
          //  Debug.LogWarning("Error: " + e.Message);
        }
    }

    byte[] loadImage(string path)
    {
        byte[] dataByte = null;

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
           // Debug.LogWarning("Directory does not exist");
            return null;
        }

        if (!File.Exists(path))
        {
           // Debug.Log("File does not exist");
            return null;
        }

        try
        {
            dataByte = File.ReadAllBytes(path);
          //  Debug.Log("Loaded Data from: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
           // Debug.LogWarning("Failed To Load Data from: " + path.Replace("/", "\\"));
           // Debug.LogWarning("Error: " + e.Message);
        }

        return dataByte;
    }

    public void PressButton()
    {
            canvas.transform.Find(itemID).gameObject.SetActive(true);
            
    }

    // Update is called once per frame
    void Update () {
	}
}
