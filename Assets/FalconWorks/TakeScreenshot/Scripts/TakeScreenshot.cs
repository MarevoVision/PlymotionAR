using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Runtime.InteropServices;
using DG.Tweening;


public class TakeScreenshot : MonoBehaviour {

	// --------
	#region iOS Native Plugin Methods
	[DllImport("__Internal")] private static extern void _PlaySystemShutterSound ();

	[DllImport("__Internal")] private static extern void _CallSavingPhotoEvent (string parentObjName, string path, string callbackName);
	#endregion

	// --------
	#region インスペクタ設定用フィールド
	/// <summary>
	/// Android用写真フォルダ名格納フィールド
	/// </summary>
	public string m_ForAndroid_MediaDirName = "";
	/// <summary>
	/// 写真のファイル名
	/// </summary>
	public string m_FileName = "";

	[Header("*UI : CapturePreview")]
	/// <summary>
	/// ランドスケープ用撮影プレビュー
	/// </summary>
	public GameObject m_CapturePreviewLandscape;
	/// <summary>
	/// ポートレイト用撮影プレビュー
	/// </summary>
	public GameObject m_CapturePreviewPortrait;
	[Header("*UI : PreviewFrame")]
	/// <summary>
	/// ランドスケープ用撮影プレビューフレーム
	/// </summary>
	public GameObject m_PreviewFrameLandscape;
	/// <summary>
	/// ポートレイト用撮影プレビューフレーム
	/// </summary>
	public GameObject m_PreviewFramePortrait;

	[Header("*UI : CaptureSprite")]
	/// <summary>
	/// ランドスケープ用プレビューイメージ
	/// </summary>
	public RawImage m_CaptureRawImageLandscape;
	/// <summary>
	/// ポートレイト用プレビューイメージ
	/// </summary>
	public RawImage m_CaptureRawImagePortrait;

	[Header("*UI : CaptureSpriteFitter")]
	/// <summary>
	/// ランドスケープ用プレビューイメージのアスペクトフィッター
	/// </summary>
	public AspectRatioFitter m_CaputureSpireteFitterLandscape;
	/// <summary>
	/// ポートレイト用プレビューイメージのアスペクトフィッター
	/// </summary>
	public AspectRatioFitter m_CaputureSpireteFitterPortrait;

	[Header("*UI : DialogSuccess")]
	/// <summary>
	/// ランドスケープ用保存成功ダイアログ
	/// </summary>
	public GameObject m_DialogSuccessLandscape;
	/// <summary>
	/// ポートレイト用保存成功ダイアログ
	/// </summary>
	public GameObject m_DialogSuccessPortrait;

	[Header("*UI : PermissionMessage")]
	/// <summary>
	/// 写真パーミッションアラート本体
	/// </summary>
	public Transform m_PhotoLibraryPermissionContent;
	/// <summary>
	/// 写真パーミッションアラートのカンバスグループ
	/// </summary>
	public CanvasGroup m_PhotoLibraryPermissionCanvasGroup;
	/// <summary>
	/// 写真パーミッションアラートのタイトル
	/// </summary>
	public Text m_PhotoLibraryPermissionTitle;
	/// <summary>
	/// 写真パーミッションアラートのメッセージ
	/// </summary>
	public Text m_PhotoLibraryPermissionMessage;
	/// <summary>
	/// 写真パーミッションアラート上に表示させるアプリ名
	/// </summary>
	public string m_AppName = "ARCoreList";

	[Header("*UI : ShutterFlash")]
	/// <summary>
	/// シャッターフラッシュエフェクト用カンバス
	/// </summary>
	public GameObject m_FlashCanvas;
	/// <summary>
	/// シャッターフラッシュエフェクト用カンバスグループ
	/// </summary>
	public CanvasGroup m_FlashCanvasGroup;

	[Header("*Auido")]
	/// <summary>
	/// オーディミキサー
	/// </summary>
	public AudioMixer m_AudioMixer;
	/// <summary>
	/// The default snapshot.
	/// </summary>
	public AudioMixerSnapshot m_DefaultSnapshot;
	/// <summary>
	/// The screen capture snapshot.
	/// </summary>
    public AudioMixerSnapshot m_CapturePreviewSnapshot;
	#endregion



	// --------
	#region メンバーフィールド
	private bool m_IsTakingCapture = false;
	private bool m_IsSaving = false;
	private string m_FileType = ".jpg";
	private Texture2D m_TempScreenShot;
	/// <summary>
	/// オーディミキサースナップショット格納用配列
	/// </summary>
	private AudioMixerSnapshot[] m_AudioMixerSnapshots;

	/// <summary>
	/// delegate型を宣言
	/// </summary>
	public delegate void OnComplete(); 	// delegate
	protected OnComplete callBack;		// コールバック
	#endregion

	// -----
	#region インナークラス
	/// <summary>
	/// AL assets library error codes.
	/// </summary>
	public class ALAssetsLibraryErrorCodes {
		/// <summary>
		/// The reason for the error is unknown.
		/// </summary>
		public const string ALAssetsLibraryUnknownError = "-1";
		/// <summary>
		/// The attempt to write data failed.
		/// </summary>
		public const string ALAssetsLibraryWriteFailedError = "-3300";
		/// <summary>
		/// The AL assets library write busy error.
		/// </summary>
		public const string ALAssetsLibraryWriteBusyError = "-3301";
		/// <summary>
		/// The AL assets library write invalid data error.
		/// </summary>
		public const string ALAssetsLibraryWriteInvalidDataError = "-3302";
		/// <summary>
		/// The AL assets library write incompatible data error.
		/// </summary>
		public const string ALAssetsLibraryWriteIncompatibleDataError = "-3303";
		/// <summary>
		/// The AL assets library write data encoding error.
		/// </summary>
		public const string ALAssetsLibraryWriteDataEncodingError = "-3304";
		/// <summary>
		/// The AL assets library write disk space error.
		/// </summary>
		public const string ALAssetsLibraryWriteDiskSpaceError = "-3305";
		/// <summary>
		/// The AL assets library data unavailable error.
		/// </summary>
		public const string ALAssetsLibraryDataUnavailableError = "-3310";
		/// <summary>
		/// The AL assets library access user denied error.
		/// </summary>
		public const string ALAssetsLibraryAccessUserDeniedError = "-3311";
		/// <summary>
		/// The AL assets library access user denied error.
		/// </summary>
		public const string ALAssetsLibraryAccessGloballyDeniedError = "-3312";

	}
	#endregion

	// --------
	#region MonoBehaviourメソッド
	/// <summary> 
	/// 初期化処理
	/// </summary>
	void Awake(){
		
	}

	/// <summary>
	/// 開始処理
	/// </summary>
	void Start () {

		//各UIコンポーネントの初期化
		m_CapturePreviewLandscape.SetActive (false);
		m_CapturePreviewPortrait.SetActive (false);

		m_DialogSuccessLandscape.SetActive (true);
		m_DialogSuccessLandscape.transform.DOScale (new Vector3 (0.5f, 0.5f, 0.5f), 0.0f);
		m_DialogSuccessLandscape.GetComponent<Image> ().DOFade (0.0f, 0.0f);

		m_DialogSuccessPortrait.SetActive (true);
		m_DialogSuccessPortrait.transform.DOScale (new Vector3 (0.5f, 0.5f, 0.5f), 0.0f);
		m_DialogSuccessPortrait.GetComponent<Image> ().DOFade (0.0f, 0.0f);

		m_FlashCanvas.SetActive(false);

		//スナップショットの配列の初期化
		m_AudioMixerSnapshots = new AudioMixerSnapshot[2] { m_CapturePreviewSnapshot, m_DefaultSnapshot };
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	void Update () {
		
	}
	#endregion

	//----------
	#region メンバーメソッド
	/// <summary>
	/// Capture this instance.
	/// </summary>
	public void TakeCapture(){

        Camera.main.cullingMask = -1;
        m_PreviewFrameLandscape.SetActive (false);
		m_PreviewFramePortrait.SetActive (false);

		if (m_IsTakingCapture) {
			//Debug.Log ("撮影中につき、通行止め");
			return;
		}

		switch (Screen.orientation) {
		case ScreenOrientation.Portrait:
			Screen.orientation = ScreenOrientation.Portrait;
			break;
		case ScreenOrientation.PortraitUpsideDown:
			Screen.orientation = ScreenOrientation.PortraitUpsideDown;
			break;
		case ScreenOrientation.LandscapeLeft:
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			break;
		case ScreenOrientation.LandscapeRight:
			Screen.orientation = ScreenOrientation.LandscapeRight;
			break;
		}

		m_IsTakingCapture = true;
		StartCoroutine (GeneratePreviewImage()); //画像生成

		//シャッター音再生
		#if UNITY_IOS && !UNITY_EDITOR
		_PlaySystemShutterSound ();
		#elif UNITY_ANDROID && !UNITY_EDITOR
		var mediaActionSound = new AndroidJavaObject("android.media.MediaActionSound");
		mediaActionSound.Call("play", mediaActionSound.GetStatic<int>("SHUTTER_CLICK"));
		#endif

		//フラッシュ演出
		m_FlashCanvas.SetActive(true);
		m_FlashCanvasGroup.alpha = 1.0f;
		m_FlashCanvasGroup.DOFade (0f, 0.8f).SetDelay(0.3f).OnComplete(()=>{
			
			m_FlashCanvas.SetActive(false);

			switch (Screen.orientation) {
			case ScreenOrientation.Portrait:
			case ScreenOrientation.PortraitUpsideDown:
				m_CapturePreviewLandscape.SetActive(false);
				m_CapturePreviewPortrait.SetActive(true);
				break;
			case ScreenOrientation.LandscapeLeft:
			case ScreenOrientation.LandscapeRight:
				m_CapturePreviewLandscape.SetActive(true);
				m_CapturePreviewPortrait.SetActive(false);
				break;
			}

		});

    }


	/// <summary>
	/// Generates the preview image.
	/// </summary>
	/// <returns>The preview image.</returns>
	IEnumerator GeneratePreviewImage(){

		yield return new WaitForEndOfFrame ();

		m_TempScreenShot = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
		RenderTexture _renderTexture = new RenderTexture(Screen.width, Screen.height,24);

		int cameraNum = Camera.allCamerasCount;
		for(int index = 0; index< cameraNum; index ++){
			Camera cam = Camera.allCameras [index];


			if((cam.cullingMask) == 0){
				continue;
			}
			RenderTexture prev = cam.targetTexture;
			cam.targetTexture = _renderTexture;
			cam.Render ();
			cam.targetTexture = prev;
		}

		RenderTexture.active = _renderTexture;
		m_TempScreenShot.ReadPixels (new UnityEngine.Rect(0,0,m_TempScreenShot.width, m_TempScreenShot.height), 0, 0);
		m_TempScreenShot.Apply();

		yield return new WaitForEndOfFrame ();

        //SNS投稿用の画像を生成--------------------------

        string saveFileName = Application.persistentDataPath + "/Screenshot" + m_FileType;
		byte[] jpgData = m_TempScreenShot.EncodeToJPG (50);

        #if UNITY_EDITOR
        //Debug.Log("Editorだよ");
        File.WriteAllBytes(saveFileName , jpgData);
		yield return new WaitForEndOfFrame ();
		OnCompleteCapture();

        #elif UNITY_IOS
        File.WriteAllBytes(saveFileName , jpgData);
		#elif UNITY_ANDROID
		File.WriteAllBytes(saveFileName , jpgData);
		#endif
		//-------------------------------------------

		Destroy (_renderTexture);

		//画面が縦だったら横向き用のspriteを非表示にする
		float ratio = 0;
		switch (Screen.orientation) {
		case ScreenOrientation.Portrait:
		case ScreenOrientation.PortraitUpsideDown:

			m_PreviewFrameLandscape.SetActive (false); 
			m_PreviewFramePortrait.SetActive (true);
			
			m_CaptureRawImagePortrait.texture = m_TempScreenShot;
			ratio = (float)m_TempScreenShot.width / m_TempScreenShot.height;
			m_CaputureSpireteFitterPortrait.aspectRatio = ratio;

			break;

		case ScreenOrientation.LandscapeLeft:
		case ScreenOrientation.LandscapeRight:

			m_PreviewFrameLandscape.SetActive (true);
			m_PreviewFramePortrait.SetActive (false);

			m_CaptureRawImageLandscape.texture = m_TempScreenShot;
			ratio = (float)m_TempScreenShot.width / m_TempScreenShot.height;
			m_CaputureSpireteFitterLandscape.aspectRatio = ratio;

			break;
		}

		StartCoroutine (AudioSourceStop ());

        NativeGallery.SaveImageToGallery(jpgData, "ARCoreList", "Screenshot ARCoreList {0}.jpg");
        OnCompleteCapture();
        m_PreviewFramePortrait.SetActive(false);
        //OnSave();
    }
	
	/// <summary>
	/// Audios the source stop.
	/// </summary>
	IEnumerator AudioSourceStop(){
		yield return new WaitForSeconds (0.1f);

		float[] weights = { 1.0f, 0.0f };
		m_AudioMixer.TransitionToSnapshots(m_AudioMixerSnapshots, weights, 0.5f);
	}

	// TODO
	/// <summary>
	/// 保存失敗時の処理
	/// </summary>
	private void OnCaptureFailed(string  _message=""){

		string title = "Save failed";
		string message = "";

		#if UNITY_IOS
		message = _message;
		#elif UNITY_ANDROID
		message = "Access to the storage is not allowed.\n\n「Settings」From within the application of\n「" + m_AppName+ "」To 'storage'\n Please turn on access.";
		#endif

		m_PhotoLibraryPermissionTitle.text = title;
		m_PhotoLibraryPermissionMessage.text = message;

		//アラートを表示
		openPhotoLibraryAlertAnimation ();
		m_IsSaving = false;

	}

	/// <summary>
	/// アラートを閉じます。
	/// </summary>
	public void closePhotoLibraryAlert (){
	// 閉じるアニメーション
		closePhotoLibraryAlertAnimation ();
	}

	/// <summary>
	/// アラート表示時のアニメーション処理です。
	/// </summary>
	private void openPhotoLibraryAlertAnimation (){

		// アクティベート
		onPhotoLibraryAlertInitialize ();
		// 事前に初期値に合わせる
		m_PhotoLibraryPermissionContent.localScale = new Vector3 (0.9f, 0.9f, 0.9f);
		m_PhotoLibraryPermissionCanvasGroup.alpha = 0f;
		// アニメーション
		m_PhotoLibraryPermissionContent.DOScale (Vector3.one, 0.25f).SetEase (Ease.OutExpo);
		DOTween.To (() => m_PhotoLibraryPermissionCanvasGroup.alpha, alpha => m_PhotoLibraryPermissionCanvasGroup.alpha = alpha, 1f, 0.25f);

	}

	/// <summary>
	/// アラート表示時のアニメーション処理です。
	/// </summary>
	private void closePhotoLibraryAlertAnimation (){

		// 事前に初期値に合わせる
		m_PhotoLibraryPermissionContent.localScale = Vector3.one;
		m_PhotoLibraryPermissionCanvasGroup.alpha = 1f;
		// アニメーション
		m_PhotoLibraryPermissionContent.DOScale (new Vector3 (0.9f, 0.9f, 0.9f), 0.25f).SetEase (Ease.OutExpo).OnComplete (()=>{onPhotoLibraryAlertFinalize();});
		DOTween.To (() => m_PhotoLibraryPermissionCanvasGroup.alpha, alpha => m_PhotoLibraryPermissionCanvasGroup.alpha = alpha, 0f, 0.25f);

	}

	/// <summary>
	/// アラート表示時に呼び出される
	/// </summary>
	private void onPhotoLibraryAlertInitialize (){

		// CanvasGroupを取得する
		if (m_PhotoLibraryPermissionCanvasGroup == null) m_PhotoLibraryPermissionCanvasGroup = m_PhotoLibraryPermissionContent.GetComponent<CanvasGroup> ();
		// 有効化する
		m_PhotoLibraryPermissionContent.gameObject.SetActive (true);

	}

	/// <summary>
	/// アラート終了後に呼び出される
	/// </summary>
	private void onPhotoLibraryAlertFinalize (){
		// 停止させる
		m_PhotoLibraryPermissionContent.gameObject.SetActive (false);
	}

	/// <summary>
	/// Raises the complete capture event.
	/// </summary>
	private void OnCompleteCapture(){

		m_DialogSuccessLandscape.transform.DOScale (Vector3.one, 0.5f).SetEase (Ease.OutBack);
		m_DialogSuccessLandscape.GetComponent<Image> ().DOFade (1.0f,0.5f);

		m_DialogSuccessPortrait.transform.DOScale (Vector3.one, 0.5f).SetEase (Ease.OutBack);
		m_DialogSuccessPortrait.GetComponent<Image> ().DOFade (1.0f,0.5f);


		StartCoroutine ( CompleteDelay() );


	}


	IEnumerator CompleteDelay(){

		// 効果音再生
//		SoundManager.Instance.PlaySe ("se_loadcomplete");

		m_DialogSuccessLandscape.transform.DOScale(new Vector3(0.5f,0.5f,0.5f),  0.5f).SetDelay(1.5f).SetEase(Ease.InBack);
		m_DialogSuccessLandscape.GetComponent<Image> ().DOFade (0.0f, 0.5f).SetDelay(1.5f).OnComplete(()=>{});

		m_DialogSuccessPortrait.transform.DOScale(new Vector3(0.5f,0.5f,0.5f),  0.5f).SetDelay(1.5f).SetEase(Ease.InBack);
		m_DialogSuccessPortrait.GetComponent<Image> ().DOFade (0.0f, 0.5f).SetDelay(1.5f).OnComplete(()=>{});

		yield return new WaitForSeconds (2.2f);

		Debug.Log ("You have finished all the save process");

		m_IsTakingCapture = false;
		m_IsSaving = false;

		Destroy(m_TempScreenShot);

		m_CaptureRawImageLandscape.texture = null;
		m_CaptureRawImagePortrait.texture = null;

		//Screen.orientation = ScreenOrientation.AutoRotation;

		m_CapturePreviewLandscape.SetActive (false);
		m_CapturePreviewPortrait.SetActive (false);

		//TODO
		float[] weights = { 0.0f, 1.0f };
		m_AudioMixer.TransitionToSnapshots(m_AudioMixerSnapshots, weights, 0.5f);

	}

	/// <summary>
	/// Raises the saving capture event.
	/// </summary>
	public void OnSave(){

		if (m_IsSaving) {
			return;
		}


		m_IsSaving = true;
		StartCoroutine (SavingCaptureImage());

	}

	/// <summary>
	/// Raises the cancel saving capture event.
	/// </summary>
	public void OnCancel(){
		
		if (m_IsSaving) {
			return;
		}

		m_IsTakingCapture = false;
		m_IsSaving = false;

		//tempScreenShot = null;
		Destroy(m_TempScreenShot);

		m_CaptureRawImageLandscape.texture = null;
		m_CaptureRawImagePortrait.texture = null;

		//Screen.orientation = ScreenOrientation.AutoRotation;

		m_CapturePreviewLandscape.SetActive (false);
		m_CapturePreviewPortrait.SetActive (false);

		float[] weights = { 0.0f, 1.0f };
		m_AudioMixer.TransitionToSnapshots(m_AudioMixerSnapshots, weights, 0.5f);
	}

	/// <summary>
	/// Savings the capture image.
	/// </summary>
	/// <returns>The capture image.</returns>
	IEnumerator SavingCaptureImage(){
		
		yield return new WaitForEndOfFrame ();

		string path = Application.persistentDataPath + "/";
		string saveFileName = m_FileName + GetDateString () + m_FileType;

		byte[] jpgData = m_TempScreenShot.EncodeToJPG();

		#if UNITY_EDITOR
		File.WriteAllBytes(saveFileName , jpgData);
		yield return new WaitForEndOfFrame ();
		OnCompleteCapture();

#elif UNITY_ANDROID
		if(UniAndroidPermission.IsPermitted (AndroidPermission.WRITE_EXTERNAL_STORAGE)){
			NativeGallery.SaveImageToGallery(jpgData, "ARCoreList", "Screenshot ARCoreList {0}.jpg");
		}else{
			OnCaptureFailed();
		}

#endif


    }



    IEnumerator AndroidNativePlugin_CallSavingPhotoEvent(string path, string fileName, byte[] jpgData){

		yield return new WaitForEndOfFrame ();

		string saveDirPath = "";

		using(AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment"))
		using(AndroidJavaObject joExDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory") ){
			saveDirPath = joExDir.Call<string> ("toString") + "/DCIM/" + m_ForAndroid_MediaDirName + "/";
			Debug.Log ("Unity::" + "This is the path. : " + saveDirPath);
		}

		if (!Directory.Exists (saveDirPath)) {
			Directory.CreateDirectory(saveDirPath);
			Debug.Log ("Unity::" + "dirIt was a great experience for me.");
		} 

		yield return new WaitForEndOfFrame ();

		File.WriteAllBytes(saveDirPath + fileName , jpgData);
		Debug.Log ("Unity::" + "savePth:" + saveDirPath + fileName);
		
		yield return new WaitForEndOfFrame ();

		ScanMedia(saveDirPath + fileName);
		if( File.Exists( path + fileName) ){
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("Unity::" + "Delete:" + path + fileName);
			File.Delete (path + fileName);
		}else{
			Debug.Log ("Unity::" + fileName + " No.");
		}

		OnCompleteCapture ();
	}


	//※「PlayerSetting」>「OtherSettings」>「WritePermission」を「External(SDcard)」に設定する
	void ScanMedia (string fileName){
		if (Application.platform != RuntimePlatform.Android) return;
		using (AndroidJavaClass jcUnityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer"))
		using (AndroidJavaObject joActivity = jcUnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity"))
		using (AndroidJavaObject joContext = joActivity.Call<AndroidJavaObject> ("getApplicationContext"))
		using (AndroidJavaClass jcMediaScannerConnection = new AndroidJavaClass ("android.media.MediaScannerConnection"))
		using (AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment"))
		using (AndroidJavaObject joExDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
		{
			jcMediaScannerConnection.CallStatic("scanFile", joContext, new string[] { fileName }, new string[] { "image/jpeg" }, null);
//			jcMediaScannerConnection.CallStatic("scanFile", joContext, new string[] { fileName }, new string[] { "image/png" }, null); //pngの場合
		}
	}




	/// <summary>
	/// IOSs the native plugin call saving photo event.
	/// </summary>
	/// <returns>The native plugin call saving photo event.</returns>
	/// <param name="fileName">File name.</param>
	IEnumerator IOSNativePlugin_CallSavingPhotoEvent(string fileName){

		yield return new WaitForSeconds (0.2f);

		Debug.Log (this.gameObject.name + " / " + fileName + " / ");
		_CallSavingPhotoEvent ( this.gameObject.name, fileName, "IOS_CallbackSavingPhotoEvent");

	}

	/// <summary>
	/// IOs the s callback saving photo event.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void IOS_CallbackSavingPhotoEvent(string callback){

		if( File.Exists( Application.persistentDataPath + "/" + callback) ){
			File.Delete (Application.persistentDataPath + "/" + callback);
			//Debug.Log (callback + " を削除したよ");
			OnCompleteCapture();
		}else{
			if (callback == ALAssetsLibraryErrorCodes.ALAssetsLibraryAccessUserDeniedError) {
				//フォトライブラリへのアクセスが許可されていない
				//TODO
				OnCaptureFailed ("Access to photos is not allowed.\n\n「Settings」From「" + m_AppName + "」Choose the、\n「Settings」Add only photos「permissions」\nPlease set it to.");

			} else if (callback == ALAssetsLibraryErrorCodes.ALAssetsLibraryWriteDiskSpaceError) {
				//データを書き込むのに十分な容量がなかった
				OnCaptureFailed ("There is no free space required to store photos in the storage.");
			} else {
				//書き込み失敗等
				OnCaptureFailed ();
			}
		}

	}


	/// <summary>
	/// 文字列で日時を取得
	/// </summary>
	/// <returns> yyyyMMddHHmmss </returns>
	private string GetDateString(){
		return DateTime.Now.ToString ("yyyyMMddHHmmss");
	}
	#endregion

}