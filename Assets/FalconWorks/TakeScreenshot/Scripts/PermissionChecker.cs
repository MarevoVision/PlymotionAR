using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.InteropServices;

public class PermissionChecker : MonoBehaviour {

	string callback_useCamera_method = "CallbackUseCamera";
	string callback_usePhotoLibrary_method = "CallbackUsePhotoLibrary";


	//
	[SerializeField, HeaderAttribute ("Call back when camera permission is OK")]
	public UnityEvent callback_useCameraAction;

	//
	[SerializeField, HeaderAttribute ("Callbacks for camera permission errors")]
	public UnityEvent callback_useCameraAction_Error;

	//
	[SerializeField, HeaderAttribute ("Callback when a permission error occurs in the photo library")]
	public UnityEvent callback_usePhotoLibraryAction_Error;

	#if UNITY_IOS
	[DllImport("__Internal")]
	private static extern void _PermissionCheck_Use_Camera (string parentObjName, string callbackName);

	[DllImport("__Internal")]
	private static extern void _PermissionCheck_Use_PhotoLibrary (string parentObjName, string callbackName);
	#endif

	private bool isCameraAllow = false;
	private bool isPhotoLibraryAllow = false;


	//カメラのパーミッションを確認する
	//※コールバックはPermissionCheckerのUnityEventの登録
	public void OnCheckPermissionCamera(){
	#if UNITY_IOS && !UNITY_EDITOR
		_PermissionCheck_Use_Camera (this.gameObject.name, callback_useCamera_method);
	#elif UNITY_ANDROID

	#endif
	}



	//フォトライブラリのパーミッションを確認する
	//※コールバックはPermissionCheckerのUnityEventの登録
	public void OnCheckPermissionPhotoLibrary(){

	#if UNITY_IOS && !UNITY_EDITOR
		_PermissionCheck_Use_PhotoLibrary (this.gameObject.name, callback_usePhotoLibrary_method);
	#elif UNITY_ANDROID

	#endif
	}

	public bool IsGetUsingCamera(){
		return isCameraAllow;
	}

	public bool IsGetUsingPhotoLibrary(){
		return isPhotoLibraryAllow;
	}


	#region IOS_NativeCallback
	#if UNITY_IOS

	/// <summary>
	/// Callbacks the use camera.
	/// </summary>
	/// <param name="callback">Callback.</param>
	private void CallbackUseCamera(string callback){
		if (callback == "false") {
			Debug.Log ("The camera is not allowed");
			isCameraAllow = false;
			if(callback_useCameraAction_Error != null)
				callback_useCameraAction_Error.Invoke ();
		} else {
			Debug.Log ("The camera is allowed");
			isCameraAllow = true; 
			if (callback_useCameraAction != null)
				callback_useCameraAction.Invoke ();
		}
	}


	/// <summary>
	/// Callbacks the use photo library.
	/// </summary>
	/// <param name="callback">Callback.</param>
	private void CallbackUsePhotoLibrary(string callback){

		if (callback == "false") {
			isPhotoLibraryAllow = false;
			if (callback_usePhotoLibraryAction_Error != null)
				callback_usePhotoLibraryAction_Error.Invoke ();			
		} else {
			Debug.Log ("Unity::Album callbackOK");
			isPhotoLibraryAllow = true;
		}
	}
	#endif
	#endregion


	public void TempPermissionCallback(){
		Debug.Log ("Unity::callbackNGNG");
		
	}




}
