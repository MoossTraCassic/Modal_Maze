using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
{
    Scene scene;


  void loadScene()
    {
        scene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(scene.buildIndex + 1);

    }
}
