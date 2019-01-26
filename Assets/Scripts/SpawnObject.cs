using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    private string modelID;
    public int textureLength;
    public string modelLink;
    
    public string savePathModel, savePathModelMTL;

    // Use this for initialization
    void Start () {
        
        modelID = PlayerPrefs.GetString("modelID");

        modelLink = PlayerPrefs.GetString("modelLink" + modelID);
        textureLength = PlayerPrefs.GetInt("modelTexture" + modelID) - 1;

        savePathModel = Path.Combine(Application.persistentDataPath, "data");
        savePathModel = Path.Combine(savePathModel, "Model" + modelID);
        savePathModel = Path.Combine(savePathModel, Path.GetFileName(modelLink));

        savePathModelMTL = Path.Combine(Application.persistentDataPath, "data");
        savePathModelMTL = Path.Combine(savePathModelMTL, "Model" + modelID);
        savePathModelMTL = Path.Combine(savePathModelMTL, Path.GetFileNameWithoutExtension(modelLink) + ".mtl");

        //meshRenderer = GetComponent<MeshRenderer>();
        //StartCoroutine(SpawnOBJ());

         //ObjectLoader loader = gameObject.AddComponent<ObjectLoader>();
        // loader.Load(savePathModel + "/", Path.GetFileName(modelLink));


        //obj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        //obj.transform.SetParent(this.transform);
        //gameObject.GetComponent<MeshFilter>().mesh = FastObjImporter.Instance.ImportFile(savePathModel);
        //Material[] grassmaterial = OBJLoader.LoadMTLFile(savePathModelMTL);
        //gameObject.GetComponent<Renderer>().material = grassmaterial[0];
    }

    IEnumerator SpawnOBJ()
    {
        //GameObject grass = OBJLoader.LoadOBJFile(savePathModel);
        //grass.transform.localScale = new Vector3(1f, 1f, 1f);
        //grass.transform.position = new Vector3(0f, 0f, 0f);
        //grass.transform.SetParent(this.transform);

        //Material[] grassmaterial = OBJLoader.LoadMTLFile(savePathModelMTL);
        //grass.GetComponentInChildren<Renderer>().material = grassmaterial[0];

        //ObjectLoader loader = gameObject.AddComponent<ObjectLoader>();
        //loader.Load(savePathModel + "/", Path.GetFileName(modelLink));

        //gameObject.GetComponent<MeshFilter>().mesh = FastObjImporter.Instance.ImportFile(savePathModel);
        //for (int i = 1; i < textureLength; i++)
        //{
        //    Material[] grassmaterial = OBJLoader.LoadMTLFile(savePathModelMTL);
        //    mats.Add(grassmaterial[i - 1]);
        //}
        //massMat = mats.ToArray();
        //gameObject.GetComponent<Renderer>().materials = mats.ToArray();
        //gameObject.SetActive(false);
        yield return null;
    }


}
