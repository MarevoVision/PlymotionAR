using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDownloadMenu : MonoBehaviour {

    public string nameBtn;
    public GameObject downloadMenu;
    // Use this for initialization
    void Start () {
        nameBtn = transform.name;
        gameObject.transform.GetComponent<Button>().onClick.AddListener(BtnPress);
        downloadMenu = GameObject.Find("/Canvas" + "/" + nameBtn + "/dwn");
    }

    // Update is called once per frame
    void Update () {
	}

    public void BtnPress()
    {
        downloadMenu.SetActive(true);
    }
}
