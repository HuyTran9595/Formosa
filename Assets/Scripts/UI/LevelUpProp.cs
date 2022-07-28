using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpProp : MonoBehaviour
{
    public struct newUnlock
    {
        public enum Type
        {
            Item, plot, lab, magic
        }
        public Type type;
        public string name;
        public Sprite icon;

        public newUnlock(Type _type, string _name, Sprite _sprite = null)
        {
            type = _type;
            name = _name;
            icon = _sprite;
        }
    }

    public GameObject LvUpPropIcon;
    public GameObject ScrollViewContainer;
    public TMPro.TextMeshProUGUI lvTitle;
    public Sprite plotIcon;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelUp(int newLv, List<newUnlock> list)
    {
        gameObject.SetActive(true);
        lvTitle.text = "You have reached level " + newLv.ToString() + " !";
        //Turn on the prop
        //Clean all child
        foreach (Transform child in ScrollViewContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //For each on list, create a icon
        foreach (var item in list)
        {
            GameObject newIcon = Instantiate(LvUpPropIcon, ScrollViewContainer.transform);
            switch (item.type)
            {
                case newUnlock.Type.Item:
                    newIcon.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = item.name;
                    newIcon.GetComponentInChildren<UnityEngine.UI.Image>().sprite = item.icon;
                    break;
                case newUnlock.Type.plot:

                    newIcon.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "New plot in greenhouse!";
                    newIcon.GetComponentInChildren<UnityEngine.UI.Image>().sprite = plotIcon;
                    break;
                case newUnlock.Type.lab:
                    newIcon.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "New plot in Lab!";
                    newIcon.GetComponentInChildren<UnityEngine.UI.Image>().sprite = plotIcon;
                    break;
                case newUnlock.Type.magic:
                    newIcon.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "New plot in Magic Lab!";
                    newIcon.GetComponentInChildren<UnityEngine.UI.Image>().sprite = plotIcon;
                    break;
                default:
                    break;
            }
            
        }
    }
}
