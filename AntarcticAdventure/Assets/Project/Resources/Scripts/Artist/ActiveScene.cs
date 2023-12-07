using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveScene : MonoBehaviour
{
    public string SceneName;
    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
        Debug.Log(SceneManager.GetActiveScene().name);
        Debug.Log("Acitve");
    }
    void Update()
    {
        
    }
}
