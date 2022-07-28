using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeMenu : MonoBehaviour
{
    // Scene is changed to menu
    public void changeScene()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
}
