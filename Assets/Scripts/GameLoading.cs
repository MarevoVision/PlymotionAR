using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLoading : MonoBehaviour
{
    
    private AsyncOperation async;
    public string sceneName;
    private bool load;
    void Start()
    {
        load = false;
    }
    public void btnMain()
    {
        StartCoroutine(Loading());

    }

    void Update()
    {
       // if (load == true) async.allowSceneActivation = true;
    }

    IEnumerator Loading()
    {
        async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        async.allowSceneActivation = false;
        while (async.progress <= 0.89f)
        {
            yield return null;
        }
        async.allowSceneActivation = true;
    }

}