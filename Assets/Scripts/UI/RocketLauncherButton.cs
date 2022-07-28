using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherButton : MonoBehaviour
{
    RocketLauncher rl;
    bool isAtStation;
    public GameObject Map;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        if (Map != null)
            Map.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleOn(RocketLauncher rocketLauncher, bool _isAtStation)
    {
        rl = rocketLauncher;
        isAtStation = _isAtStation;
        gameObject.SetActive(true);
    }

    public void ToggleOff()
    {
        gameObject.SetActive(false);
    }

    public void OnClicked()
    {
        //If at station
        //Turn on map for option
        if (isAtStation)
        {
            gameObject.SetActive(false);
            Map.SetActive(true);
        }
        else
            rl.Teleport("Master Scene");


        //else
        //back to station

    }

    public void CloseMap()
    {
        //gameObject.SetActive(true);
        //Map.SetActive(false);
    }

    public void MoveTo(string position)
    {
        rl.Teleport(position);
    }
}
