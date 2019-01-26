using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeScreenshot2 : MonoBehaviour {

	[SerializeField]
	GameObject blink;
    public GameObject btnMenu, inMenu, shot, shotMenu;

    public void TakeAShot()
    {
        PanelClose();
        StartCoroutine("CaptureIt");
    }

	IEnumerator CaptureIt()
	{
        yield return new WaitForSeconds(0.5f);
		string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
		string fileName = "Screenshot" + timeStamp + ".png";
		string pathToSave = fileName;
		ScreenCapture.CaptureScreenshot(pathToSave);
		yield return new WaitForEndOfFrame();
		Instantiate (blink, new Vector2(0f, 0f), Quaternion.identity);
        shotMenu.SetActive(true);
    }


    public void PanelClose()
    {
        btnMenu.SetActive(false);
        inMenu.SetActive(false);
        shot.SetActive(false);
    }

    public void PanelOpen()
    {
        btnMenu.SetActive(true);
        shot.SetActive(true);
        shotMenu.SetActive(false);
    }
}
