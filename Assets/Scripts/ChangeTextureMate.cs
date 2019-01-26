using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextureMate : MonoBehaviour {

    public Texture transperentTexture;
    public Texture mainTexture;
    private int changeMTexture;
    private bool work;
	// Use this for initialization
	void Start () {
        changeMTexture = 0;
        PlayerPrefs.SetInt("changeMTexture", changeMTexture);
        work = false;
	}
	
	// Update is called once per frame
	void Update () {
        changeMTexture = PlayerPrefs.GetInt("changeMTexture");
        if (changeMTexture == 1){
            gameObject.GetComponent<MeshRenderer>().material.mainTexture = transperentTexture;
            changeMTexture = 2;
            PlayerPrefs.SetInt("changeMTexture", changeMTexture);
        }
        if(changeMTexture == 0 && work == false){
            gameObject.GetComponent<MeshRenderer>().material.mainTexture = mainTexture;
            work = true;
        }
	}
}
