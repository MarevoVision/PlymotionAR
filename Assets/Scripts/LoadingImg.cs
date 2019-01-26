using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImg : MonoBehaviour {

    private ViewListItem ViewListItem = new ViewListItem();
    Texture2D tex;
    private string backPath;
    // Use this for initialization
    void Start () {
        TextAsset asset = Resources.Load("propertisAndroid") as TextAsset;
        ViewListItem = JsonUtility.FromJson<ViewListItem>(asset.text);
        for (int i = 1; i <= ViewListItem.Items_Count; i++)
        {
            backPath = ViewListItem.List[i - 1].Thumbnail;

            StartCoroutine(LoadBack(backPath));

            gameObject.transform.GetComponent<Image>().material.mainTexture = tex;
        }
    }

    private IEnumerator LoadBack(string path)
    {
        yield return 0;
        WWW link = new WWW(path);
        yield return link; // loading
        tex = link.texture;
        // GameObject obj;
        //obj.GetComponent<Renderer>().material.mainTexture = www.texture;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
