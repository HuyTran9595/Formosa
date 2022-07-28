using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketLauncher : MonoBehaviour
{
    RocketLauncherButton button;
    bool isAtStation;
    private GameObject RocketUnlockedMessage;
    string sceneName;
    // Start is called before the first frame update

    private void Awake()
    {
        button = FindObjectOfType<RocketLauncherButton>();
        if (button == null)
            Debug.LogError("Rocket cant find button");
        sceneName = SceneManager.GetActiveScene().name;
        isAtStation = (sceneName == "Master Scene") || (sceneName == "Huy_Test") || (sceneName == "Huy_Test_Master");
        //isAtStation = SceneManager.GetActiveScene().name == "Master Scene"  ? true : false;
    }


    void Start()
    {
        HandleRocketWithScenes();
        //Debug.Log(name);
    }

    private void HandleRocketWithScenes()
    {
        RocketUnlockedMessage = GameObject.Find("Rocket Unlocked Message");
        if(RocketUnlockedMessage == null)
        {
            Debug.Log("Cannot find rocket unlocked message in " + name + ". If this is not Master Scene, ignore the error");
        }

        if (RocketUnlockedMessage == null && sceneName == "Master Scene")
        {
            Debug.Log("Cannot find rocket unlocked message in " + name + ". If this is not Master Scene, ignore this error.");
        }
        else
        {
            if (RocketUnlockedMessage != null)
            {
                RocketUnlockedMessage.SetActive(false);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //button.ToggleOn(this, isAtStation);
    }

    private void OnTriggerExit(Collider other)
    {
        //button.ToggleOff();
    }

    public void Teleport(string sceneName)
    {
        // SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(sceneName);
    }

    //activate message when clicked
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)){
            if (RocketUnlockedMessage != null)
            {
                RocketUnlockedMessage.SetActive(true);
            }

            if (!isAtStation)
            {
                Debug.Log("Not in master scene, Not moving to master scene.");
                //Teleport("Master Scene");
            }
        }
    }
}
