using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveToList : MonoBehaviour {
    
    public string modelID;
    // Use this for initialization
    void Start ()
    {
        modelID = PlayerPrefs.GetString("modelID");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToList()
    {
        PlayerPrefs.SetString("removeModelID", modelID);
    }
}
