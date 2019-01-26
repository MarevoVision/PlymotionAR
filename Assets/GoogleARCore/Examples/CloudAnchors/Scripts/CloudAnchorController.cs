//-----------------------------------------------------------------------
// <copyright file="CloudAnchorController.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.CloudAnchors
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.CrossPlatform;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controller for the Cloud Anchors Example.
    /// </summary>
    public class CloudAnchorController : MonoBehaviour
    {
        [Header("ARKit")]

        /// <summary>
        /// The root for ARKit-specific GameObjects in the scene.
        /// </summary>
        public GameObject ARKitRoot;

        /// <summary>
        /// The first-person camera used to render the AR background texture for ARKit.
        /// </summary>
        public Camera ARKitFirstPersonCamera;

        /// <summary>
        /// An Andy Android model to visually represent anchors in the scene; this uses
        /// standard diffuse shaders.
        /// </summary>
        public GameObject ARKitAndyAndroidPrefab;

        public GameObject spawnOBJ;
        public GameObject SearchingForPlaneUI, btnCamera;

        bool instance = true;
        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A helper object to ARKit functionality.
        /// </summary>
        private ARKitHelper m_ARKit = new ARKitHelper();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        /// <summary>
        /// The last placed anchor.
        /// </summary>
        private Component m_LastPlacedAnchor = null;

        /// <summary>
        /// The last resolved anchor.
        /// </summary>
        private XPAnchor m_LastResolvedAnchor = null;

        /// <summary>
        /// The Unity Start() method.
        /// </summary>
        public void Start()
        {
            _ResetStatus();
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();

            // If the player has not touched the screen then the update is complete.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }
                // Raycast against the location the player touched to search for planes.
                if (Application.platform != RuntimePlatform.IPhonePlayer)
                {
                    TrackableHit hit;
                    if (Frame.Raycast(touch.position.x, touch.position.y,
                            TrackableHitFlags.PlaneWithinPolygon, out hit))
                    {
                        m_LastPlacedAnchor = hit.Trackable.CreateAnchor(hit.Pose);
                    }
                }
                else
                {
                    Pose hitPose;
                    if (m_ARKit.RaycastPlane(ARKitFirstPersonCamera, touch.position.x, touch.position.y, out hitPose))
                    {
                        m_LastPlacedAnchor = m_ARKit.CreateAnchor(hitPose);
                    }
                }

                if (m_LastPlacedAnchor != null)
                {
                    SearchingForPlaneUI.SetActive(false);
                    btnCamera.SetActive(true);
                    spawnOBJ.SetActive(true);

                    // Instantiate Andy model at the hit pose.
                    //var andyObject = Instantiate(_GetAndyPrefab(), m_LastPlacedAnchor.transform.position,
                    //    m_LastPlacedAnchor.transform.rotation);
                    ARKitAndyAndroidPrefab.transform.position = m_LastPlacedAnchor.transform.position;
                    ARKitAndyAndroidPrefab.transform.rotation = m_LastPlacedAnchor.transform.rotation;

                    // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                    //andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                    ARKitAndyAndroidPrefab.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                    // Make Andy model a child of the anchor.
                    ARKitAndyAndroidPrefab.transform.parent = m_LastPlacedAnchor.transform;
                    //andyObject.transform.parent = m_LastPlacedAnchor.transform;
                }


        }

        /// <summary>
        /// Resets the internal status and UI.
        /// </summary>
        private void _ResetStatus()
        {
            // Reset internal status.
            if (m_LastPlacedAnchor != null)
            {
                Destroy(m_LastPlacedAnchor.gameObject);
            }

            m_LastPlacedAnchor = null;
            if (m_LastResolvedAnchor != null)
            {
                Destroy(m_LastResolvedAnchor.gameObject);
            }

            m_LastResolvedAnchor = null;
        }

        /// <summary>
        /// Gets the platform-specific Andy the android prefab.
        /// </summary>
        /// <returns>The platform-specific Andy the android prefab.</returns>
        private GameObject _GetAndyPrefab()
        {
            return ARKitAndyAndroidPrefab;
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            var sleepTimeout = SleepTimeout.NeverSleep;

#if !UNITY_IOS
            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                sleepTimeout = lostTrackingSleepTimeout;
            }
#endif

            Screen.sleepTimeout = sleepTimeout;

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
