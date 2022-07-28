using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuySellButtons : MonoBehaviour
{
    // Start is called before the first frame update
    public Image myImage = null;
    public Text myText = null;
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void MakeActive()
    {

        gameObject.SetActive(true);
        if (myImage != null)
        {
            myImage.gameObject.SetActive(true);
        }

        if (myText != null)
        {
            myText.gameObject.SetActive(true);
        }

    }

    public void SetText(string newText)
    {
        if (myText != null)
        {
            myText.text = newText;
        }
    }


}
