using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RocketMapOption : MonoBehaviour
{
    public string sceneToLoad;
    Button mapButton;
    RocketLauncher rocketLauncher;
    private void Start()
    {
        mapButton = gameObject.GetComponent<Button>();
        if(mapButton == null)
        {
            Debug.Log("Null map button in " + name);
        }
        rocketLauncher = GameObject.FindObjectOfType<RocketLauncher>();
        if(rocketLauncher == null)
        {
            Debug.Log("Null rocket launcher in " + name);
        }
        //Debug.Log("Rocket launcher obj is: " + rocketLauncher.name);

        if(mapButton != null && rocketLauncher != null)
        {
            mapButton.onClick.AddListener(LoadScene);
            //Debug.Log("Added listener in " + name);
        }
        if(gameObject.name == "Island")
        {
            sceneToLoad = "Island_World";
        }
        if(gameObject.name == "Arctic")
        {
            sceneToLoad = "Arctic_World";
        }
        
    }

    private void LoadScene()
    {
        Debug.Log("Loading scene: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
    

}
