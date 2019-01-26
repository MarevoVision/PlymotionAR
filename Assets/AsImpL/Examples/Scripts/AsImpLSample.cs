using UnityEngine;

namespace AsImpL
{
    namespace Examples
    {
        /// <summary>
        /// Demonstrate how to load a model with ObjectImporter.
        /// </summary>
        public class AsImpLSample : MonoBehaviour
        {
            private string filePath;
            public ImportOptions importOptions = new ImportOptions();
            private ObjectImporter objImporter;
            private int modelLoad;

            private string modelID;
            private string modelLink;

            private void Awake()
            {
                modelID = PlayerPrefs.GetString("modelID");
                modelLink = PlayerPrefs.GetString("modelLink" + modelID);
                //modelLoad = PlayerPrefs.GetInt("modelLoad");

#if (UNITY_ANDROID || UNITY_IPHONE)
                filePath = modelLink;
#endif
                objImporter = gameObject.AddComponent<ObjectImporter>();

            }

            private void Start()
            {
                objImporter.ImportModelAsync("MyObject", filePath, gameObject.transform, importOptions);

            }
        }
    }
}
