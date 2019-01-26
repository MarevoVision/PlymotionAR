using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.SceneManagement;
using UnityEngine.iOS;
using GoogleARCore.CrossPlatform;
using UnityEngine.XR.iOS;
using System.IO;

public class Permissions : MonoBehaviour
{
    public string sceneName;
    private UnityARSessionNativeInterface m_session;

    void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            NativeGallery.Permission permission = NativeGallery.RequestPermission();
            permission = NativeGallery.Permission.ShouldAsk;
            NativeGallery.RequestPermission();
            if (NativeGallery.CheckPermission() == NativeGallery.Permission.Granted)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            NativeGallery.Permission permission = NativeGallery.RequestPermission();
            permission = NativeGallery.Permission.ShouldAsk;
            NativeGallery.RequestPermission();
            if (NativeGallery.CheckPermission() == NativeGallery.Permission.Granted)
            {
                SceneManager.LoadScene(sceneName);
            }
        }

    }

    void Update()
    {
        if (NativeGallery.CheckPermission() == NativeGallery.Permission.Granted)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
