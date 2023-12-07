using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(load());
            Debug.Log("Press");
        }
    }
    IEnumerator load()
    {
        SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
}
