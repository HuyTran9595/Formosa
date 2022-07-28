using System.Collections;
using System.Collections.Generic;
using TJayEnums;
using UnityEngine;
using UnityEngine.UI;
public class Journal : MonoBehaviour
{
    public Text viewing;
    public Text selectedName;
    public Text selectedGenus;
    public Text selectedTemp;
    public Text selectedGrowthTime;
    public Text selectedDescription;
    public RowItem selectedItem = null;
    public GameObject journalContent = null;
    public Inventory playerInventory = null;
    public List<Button> selectedButtons = new List<Button>();
    public GameObject claimButton = null;
    private Level playerLeveler = null;
    private GlowButton glow = null;
    public GameObject JournalPanel = null;

    List<int>[] unlockedDatas;  // save all unlocked item's ID
    private void Start()
    {
        unlockedDatas = new List<int>[7];
        for (int i = 0; i < unlockedDatas.Length; i++)
        {
            unlockedDatas[i] = new List<int>();
        }
    }
    private void OnEnable()
    {
        ShowNone();
        if(playerLeveler == null)
        {
            playerLeveler = GameObject.FindObjectOfType<Level>();
        }

        if (glow == null)
        {
            glow = GameObject.FindObjectOfType<GlowButton>();
        }
    }

    public void ShowNone()
    {

        selectedName.text = "";
        selectedGenus.text = "";
        selectedTemp.text = "";
        selectedGrowthTime.text = "";
        selectedDescription.text = "";
        selectedItem = null;
        if (claimButton != null)
        {
            claimButton.gameObject.SetActive(false);
        }
    }

    public void ForceUpdateContent(int newList)
    {
        if (playerInventory == null)
        {
            playerInventory = GameObject.FindObjectOfType<Inventory>();
        }

        if (playerInventory != null)
        {
            playerInventory.ForceUpdateContent(journalContent, (ListType)newList);
        }

        if (viewing != null)
        {
            viewing.text = "" + (ListType)newList;
            if(newList == (int)ListType.Tasks)
            {
                viewing.text = "Intro Tasks";
            }
        }
        UpdateButtons((int)newList);
        ShowNone();

    }
    private void UpdateButtons(int cur)
    {
        if (cur < selectedButtons.Count)
        {
            for (int i = 0; i < selectedButtons.Count; i++)
            {
                if (i == cur)
                {
                    selectedButtons[i].Select();
                    Image selectedImage = selectedButtons[i].gameObject.GetComponent<Image>();
                    if (selectedImage != null)
                    {
                        selectedImage.color = Color.white;

                        Image childImage = selectedImage.transform.GetChild(0).gameObject.GetComponent<Image>();
                        if (childImage != null)
                        {
                            childImage.color = Color.white;
                        }

                    }
                }
                else
                {
                    Color transparent = new Color(1, 1, 1, 0.5f);
                    Image selectedImage = selectedButtons[i].gameObject.GetComponent<Image>();
                    if (selectedImage != null)
                    {
                        selectedImage.color = transparent;

                        Image childImage = selectedImage.transform.GetChild(0).gameObject.GetComponent<Image>();
                        if (childImage != null)
                        {
                            childImage.color = transparent;
                        }

                    }
                }
            }
        }
    }

    public void ClaimButton()
    {
        if(selectedItem != null)
        {
            if(selectedItem.referenceData != null)
            {
                if(selectedItem.referenceData.ItemType == ItemData.Type.Task)
                {
                    if(playerLeveler != null)
                    {
                        playerLeveler.Exp(selectedItem.referenceData.Price);
                    }
                    if(playerInventory != null)
                    {
                        if(playerInventory.taskObjectives.Contains(selectedItem.referenceData))
                        {
                            playerInventory.taskObjectives.Remove(selectedItem.referenceData);
                        }
                        int nextID = (selectedItem.referenceData as TaskData).UnlockNextTaskID;
                        if (nextID > 1000)
                        {
                            ItemData nextObjective = DatabaseScript.GetItem(nextID);
                            if (!playerInventory.taskObjectives.Contains(nextObjective))
                            {
                                playerInventory.taskObjectives.Add(nextObjective);
                          
                            }
                        }
                    }
                }
            }
        }

        ForceUpdateContent((int)playerInventory.currentList);
    }

    public void Pulse()
    {
        if (glow != null)
        {
            glow.wasPressed = false;
        }
    }

    public void UnlockedItem(int itemID)
    {
        ItemData item = DatabaseScript.GetItem(itemID);
        if(item != null)
        {
            int type = (int)item.ItemType;
            List<int> list = unlockedDatas[type];
            if (list.IndexOf(itemID)==-1) //not exist
            {
                list.Add(itemID);
                list.Sort();
            }
        }
        else
        {
            Debug.Log("Error ID");
        }
    }


    void updateContent(int newList)
    {
        if(newList == (int)ListType.Tasks)
        {

        }
    }

    public void OnExitClicked()
    {
        JournalPanel.SetActive(false);
    }
}
