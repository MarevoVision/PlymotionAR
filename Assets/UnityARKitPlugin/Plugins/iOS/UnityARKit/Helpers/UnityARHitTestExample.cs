using System;
using System.Collections;
using System.Collections.Generic;
namespace UnityEngine.XR.iOS
{
	public class UnityARHitTestExample : MonoBehaviour
	{
		public Transform m_HitTransform;
		public float maxRayDistance = 30.0f;
		public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer
        public GameObject andy;
        public GameObject SearchingForPlaneUI, btnCamera;
        UnityARAnchorManager unityARAnchorManager;
        UnityARAlignment startAlignment;
        UnityARPlaneDetection planeDetection;
        public PointCloudParticleExample _pointCloudParticleExample;
        private bool stopDetection;

        bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
                    Debug.Log ("Got hit!");
                    m_HitTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                    m_HitTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                    Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                    return true;
                }
            }
            return false;
        }
        // Use this for initialization
        void Start()
        {
            unityARAnchorManager = new UnityARAnchorManager();
            stopDetection = true;
            StartCoroutine(ToturialUIAnim());
        }

        public void PlaneDetectionOFF()
        {
            planeDetection = UnityARPlaneDetection.None;
            ARKitWorldTrackingSessionConfiguration config = new ARKitWorldTrackingSessionConfiguration();
            config.planeDetection = planeDetection;
            config.alignment = startAlignment;
            config.getPointCloudData = false;
            config.enableLightEstimation = false;
            UnityARSessionNativeInterface.GetARSessionNativeInterface().RunWithConfig(config);
        }

        IEnumerator ToturialUIAnim()
        {
            yield return new WaitForSeconds(3.0f);
            SearchingForPlaneUI.SetActive(false);
            yield break;
        }

        // Update is called once per frame
        void Update () {
            //if(unityARAnchorManager.GetCurrentPlaneAnchors().Count != 0){
            //    SearchingForPlaneUI.SetActive(false);
            //}
                //#if UNITY_EDITOR   //we will only use this script on the editor side, though there is nothing that would prevent it from working on device
                // if (Input.GetMouseButtonDown (0)) {

                //    Camera.main.cullingMask -= layerMask;
                //	Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                //	RaycastHit hit;
                //	
                //we'll try to hit one of the plane collider gameobjects that were generated by the plugin
                //effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
                //	if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) {
                //we're going to get the position from the contact point
                //		m_HitTransform.position = hit.point;
                //		Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));

                //and the rotation from the transform of the plane collider
                //		m_HitTransform.rotation = hit.transform.rotation;

                //       andy.SetActive(true);
                //       btnCamera.SetActive(true);
                //   }
                // }
                //#else
            if (Input.touchCount > 0 && m_HitTransform != null && stopDetection == true)
			{
				var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

                    andy.SetActive(true);
                    btnCamera.SetActive(true);
                    //PlaneDetectionOFF();
                    //unityARAnchorManager.InvisibleAnchorPlane();
                    //unityARAnchorManager.Destroy();
                    _pointCloudParticleExample.particleSize = 0.0f;
                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = {
						//ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
						//ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
						ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    }; 
					
                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (HitTestWithResultType (point, resultType))
                        {
                            return;
                        }
                    }

				}
                stopDetection = false;
            }
//#endif

        }


    }
}

