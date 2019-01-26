using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPB : MonoBehaviour {

    public Transform LoadingBar;
    public Transform TextIndicator;
    [SerializeField]
    public float currentAmount;
    [SerializeField]
    public float speed;
    [SerializeField]
    public float maxAmout;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;
            TextIndicator.GetComponent<Text>().text = ((int)currentAmount).ToString() + "%";
        }
        else
        {
            TextIndicator.GetComponent<Text>().text = "100%";
        }
        LoadingBar.GetComponent<Image>().fillAmount = currentAmount / maxAmout;

	}
}
