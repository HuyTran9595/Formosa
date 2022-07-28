using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBarrier : MonoBehaviour
{
    public GameObject popUp;
    private bool inContact = false;
    

    private void Start()
    {
        QuestTracker.LevelUp += OnPlayerLevelUp;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Boy Animation")
        {
            inContact = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Pop up a message saying the area is out of bounds
        if (inContact == true)
        {
            popUp.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                inContact = false;
            }
        }
        else
        {
            popUp.SetActive(false);
        }
    }

    public int OnPlayerLevelUp(int currentLevel)
    {
        if (currentLevel <= 0 || currentLevel > 100)
        {
            return currentLevel;
        }
        if (currentLevel == 30)
        {
            QuestTracker.LevelUp -= OnPlayerLevelUp;
            
            Destroy(gameObject);
        }
        return currentLevel;
    }
}
