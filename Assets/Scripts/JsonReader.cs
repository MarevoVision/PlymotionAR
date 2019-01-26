using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour {

    public ViewListItem ViewListItem = new ViewListItem();
	
	void Start () {
        TextAsset asset = Resources.Load("propertisAndroid") as TextAsset;
        if(asset != null)
        {
            ViewListItem = JsonUtility.FromJson<ViewListItem>(asset.text);
            foreach (List listitem in ViewListItem.List)
            {
                
            }
        }
        else
        {
            Debug.Log("Asset is null");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
