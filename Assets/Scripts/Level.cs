using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public float experience;
    public int currentLevel = 1;
    public float levelPool;
    public Text levelText = null;
    public Slider progressBar;
    private float targetProgress;
    public float fillSpeed = 0.5f;
    private bool updating = false;
    public Shop mainShop = null;
    LevelUpProp prop;

    bool timeSpeed = false;
    UI_Notification_Manager ui_notification_manager;
    List<LevelUpProp.newUnlock> newUnlocks;


    void Awake()
    {
        GetObjects();
        experience = 0;
        levelPool = 100;
        currentLevel = 1;
        if (progressBar != null)
            progressBar.value = 0;
        Exp(0);

        if (mainShop == null)
        {
            mainShop = GameObject.FindObjectOfType<Shop>();
        }
        prop = FindObjectOfType<LevelUpProp>();
        prop.gameObject.SetActive(false);
        newUnlocks = new List<LevelUpProp.newUnlock>();
    }



    //Huy added this function to make the game automatically get the neccessary objects
    //this way we only need to update once rather than update every scene when we work on this script.
    private void GetObjects()
    {
        progressBar = GameObject.FindWithTag("Level Slider").GetComponent<Slider>();
        if(progressBar == null)
        {
            Debug.LogError("Cannot find progress bar");
        }


        levelText = GameObject.FindWithTag("Level Text").GetComponent<Text>();
        if (progressBar == null)
        {
            Debug.LogError("Cannot find Level Text");
        }


        //get the UI_Nofication_manager component
        GameObject[] ui_Notification_Managers = GameObject.FindGameObjectsWithTag("UI_Notification_Manager");
        ui_notification_manager = ui_Notification_Managers[0].GetComponent<UI_Notification_Manager>();
        if (ui_notification_manager == null)
        {
            Debug.LogError("could not find UI_Nofication_manager component");
        }
    }

#if UNITY_EDITOR
    //Noel commented this function out because playtesters noticed this auto level up button press feature in Builds
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
            Exp(999);
        Test();
    }
#endif

    //test delegate
    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            experience = levelPool;
            LevelUp();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (!timeSpeed)
            {
                Time.timeScale = 10;
                timeSpeed = true;
            }
            else
            {
                Time.timeScale = 1;
                timeSpeed = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!timeSpeed)
            {
                Time.timeScale = 99;
                timeSpeed = true;
            }
            else
            {
                Time.timeScale = 1;
                timeSpeed = false;
            }
        }
    }

    public IEnumerator IncreaseProgress()
    {

        updating = true;
        if (progressBar != null)
        {

            if (progressBar.value < targetProgress)
            {
                while (progressBar.value < targetProgress)
                {
                    if (progressBar.value >= 1)
                    {
                        break;
                    }

                    progressBar.value = Mathf.Lerp(progressBar.value, targetProgress, (fillSpeed * Time.deltaTime));
                    /*progressBar.value = fillSpeed * Time.deltaTime;*/
                    if (Mathf.Abs(progressBar.value - targetProgress) < 1 || progressBar.value >= 1)
                    {
                        progressBar.value = targetProgress;
                        break;
                    }
                    else
                    {

                    }


                    yield return null;
                }
            }

        }
        updating = false;

        if (experience > levelPool)
        {
            progressBar.value = 0;
            if (experience >= levelPool)
            {
                LevelUp();

            }
            Exp(0);
        }

        progressBar.value = experience / levelPool;

    }



    public void Exp(int exp)
    {

        experience += exp;

        // if (progressBar != null)
        //progressBar.value = experience / levelPool;

        targetProgress = experience / levelPool;
        if (updating != true)
            StartCoroutine(IncreaseProgress());
        /*progressBar.maxValue = targetProgress;*/

        if (levelText != null)
        {
            levelText.text = currentLevel.ToString();
        }
    }

    private void LevelUp()
    {
     
        if (experience >= levelPool)
        {
            currentLevel++;

            
            //Invoke QuestTracker, signifying that the player levels up
            if(QuestTracker.LevelUp != null)
            {
                QuestTracker.LevelUp(currentLevel);
            }
            //Invoke Quest System, signify that player levels up to unlock new quest
            QuestSystem questSystem = GameObject.FindWithTag("Quest System").GetComponent<QuestSystem>();
            if(questSystem == null)
            {
                Debug.LogError("Cannot find Quest system");
            }
            else
            {
                questSystem.UpdateActiveQuests(currentLevel);
            }



            //continue TJ's code
            //List<LevelUpProp.newUnlock> newUnlocks = new List<LevelUpProp.newUnlock>();
            //DatabaseScript.CheckLevelUpRewards(currentLevel, mainShop, mainShop.station.playerInventory, ref newUnlocks);
            //prop.LevelUp(currentLevel, newUnlocks);


            
            UI_LevelUpNewUnlocks();
            
            //
            experience = experience - levelPool;
            levelPool = levelPool * 1.1f;// UPDATE LEVEL POOL HERE
            if (levelText != null)
            {
                levelText.text = currentLevel.ToString();
            }
        }
    }

    private void UI_LevelUpNewUnlocks()
    {
        String message = "Level up! Current level is: " + currentLevel;
        if(currentLevel == 30)
        {
            message = "You reached level 30 and unlocked the Rocket! Check it out!";
        }
        newUnlocks = new List<LevelUpProp.newUnlock>();
        DatabaseScript.CheckLevelUpRewards(currentLevel, mainShop, mainShop.station.playerInventory, ref newUnlocks);

        ui_notification_manager.RemoveNotiButton(GetInstanceID());

        if (newUnlocks.Count > 0)
        {
            //if new unlock is not empty, we create both noti and prop UI

            ui_notification_manager.CreateNewNotiButton(OnNotificationQuitWithItems, message, GetInstanceID());
        }
        else
        {
            if(currentLevel != 30)
            {
                message += ". Nothing new is unlocked.";
            }
            ui_notification_manager.CreateNewNotiButton(OnNotificationQuitEmpty, message, GetInstanceID());
        }
        
    }

    public void OnNotificationQuitEmpty()
    {
        ui_notification_manager.OnQuitButtonClick();
        bool removed = ui_notification_manager.RemoveNotiButton(GetInstanceID());
        if (!removed)
        {
            Debug.LogError("cannot remove notification. Something is wrong.");
        }
    }

    public void OnNotificationQuitWithItems()
    {
        prop.LevelUp(currentLevel, newUnlocks);
        OnNotificationQuitEmpty();

    }
}
