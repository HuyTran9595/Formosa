using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ChestBehavior : MonoBehaviour
{
    bool isOpened = false;
    private GameObject OpenChestMessage;
    Text OpenChestMessageText;
    float timeDelayToActivateMessage = 2f;
    [SerializeField] GameObject chest_closed;
    [SerializeField] GameObject chest_opened;

    void Start()
    {
        OpenChestMessage = GameObject.Find("Open Chess Message");
        OpenChestMessageText = GameObject.Find("Open Chest Message Text").GetComponent<Text>();
        if (OpenChestMessage == null)
        {
            Debug.Log("Cannot find rocket unlocked message in " + name + ". If this is not Master Scene, ignore this error.");
        }
        else if (OpenChestMessageText == null)
        {
            Debug.Log("Cannot find open chest message text " + name);
        }
        else
        {
            OpenChestMessage.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PrintMessage();
            ActivateEndGameMessage();
        }
    }

    private void ActivateEndGameMessage()
    {
        StartCoroutine(OpenMessageAfterTime(timeDelayToActivateMessage));//open message after 1s
    }

    private void PrintMessage()
    {
        if (!isOpened) //only print the first time the chest is opened
        {
            string message = "You found the treasure chest!";
            InGameNotiManager.Instance.NewNotification(new InGameNotiManager.IngameNoti(message, gameObject));
            chest_closed.SetActive(false);
            chest_opened.SetActive(true);
            isOpened = true;
        }
    }

    IEnumerator OpenMessageAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        OpenChestMessage.SetActive(true);

        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "Arctic_World")
        {
            OpenChestMessageText.text = "Thanks for playing. You should be proud for farming all alone in space. Rest up and never stop exploring!";
        }
    }
}
