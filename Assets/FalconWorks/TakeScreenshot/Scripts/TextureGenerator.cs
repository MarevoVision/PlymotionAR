using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;



public class TextureGenerator : MonoBehaviour {

	[SerializeField, HeaderAttribute ("Events that you want to run in the callback")]
	public UnityEvent callbackEvent;

    [HideInInspector]
    public Texture2D appliedTexture;

    private Vector2[] srcPoints = new Vector2[5];


	/// <summary>
	/// Creates the texture.
	/// </summary>
	/// <param name="points">Points.</param>
	public void CreateTexture(Vector3[] points){
		for(int i = 0; i < points.Length; i++){
			srcPoints [i] = RectTransformUtility.WorldToScreenPoint (Camera.main,points[i]);
			//Debug.Log ( "Point[" + (i+1) + "] / " + srcPoints [i].x + ":" + srcPoints [i].y);
		}
		//
		StartCoroutine ( RenderTextureToTexture2D());
	}

    /// <summary>
    /// Gets the applied texture.
    /// </summary>
    /// <returns>The applied texture.</returns>
    public Texture2D GetAppliedTexture(){
        if(appliedTexture != null){
            return appliedTexture;
        }else{
            return null;
        }
    }

	/// <summary>
	/// Renders the texture to texture2d.
	/// </summary>
	/// <returns>The texture to texture2 d.</returns>
	IEnumerator RenderTextureToTexture2D(){

		yield return new WaitForEndOfFrame ();

		Texture2D tempScreenShot = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
        //Texture2D tempScreenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tempScreenShot.mipMapBias = 0.0f;
		tempScreenShot.wrapMode = TextureWrapMode.Clamp;
		//tempScreenShot.Compress(true);

		RenderTexture _renderTexture = new RenderTexture(tempScreenShot.width, tempScreenShot.height,24);

		int cameraNum = Camera.allCamerasCount;
		for(int index = 0; index< cameraNum; index ++){
			Camera cam = Camera.allCameras [index];

			if( (cam.cullingMask) == 0){
				continue;
			}
			RenderTexture prev = cam.targetTexture;
			cam.targetTexture = _renderTexture;
			cam.Render ();
			cam.targetTexture = prev;			
		}

		RenderTexture.active = _renderTexture;
		tempScreenShot.ReadPixels ( new UnityEngine.Rect(0,0,tempScreenShot.width, tempScreenShot.height), 0, 0 );
		tempScreenShot.Apply();

		yield return new WaitForEndOfFrame ();
         
        Destroy  (_renderTexture);

	}


	//end of class
}
