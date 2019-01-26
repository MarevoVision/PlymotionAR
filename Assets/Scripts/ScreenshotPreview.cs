using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScreenshotPreview : MonoBehaviour {
	
	[SerializeField]
	public GameObject canvas;
    
    public GameObject btnShot, btnCloseMenu;

    string toSave;

    // Use this for initialization
    void Start () {

    }

    private void Update()
    {
        
    }
    public void TakeAShot()
    {
        StartCoroutine(CaptureIt());
    }

    IEnumerator CaptureIt()
    {
        btnShot.SetActive(false);
        btnCloseMenu.SetActive(false);
        Camera.main.cullingMask = -1;
        yield return new WaitForSeconds(1f);
        // prepare texture with Screen and save it
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        texture.LoadRawTextureData(texture.GetRawTextureData());
        texture.Apply();
        // save to persistentDataPath File
        byte[] data = texture.EncodeToJPG(85);
        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".jpg");
        File.WriteAllBytes(destination, data);

        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        canvas.GetComponent<Image>().sprite = sp;
        canvas.SetActive(true);
        toSave = destination;

        NativeGallery.SaveImageToGallery(texture, "ARCoreList", "Screenshot ARCoreList {0}.jpg");
        
        yield return new WaitForSeconds(1f);
        canvas.SetActive(false);
        btnShot.SetActive(true);
        btnCloseMenu.SetActive(true);
    }
    
}
