using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowButton : MonoBehaviour
{
    public Image myImage;
    public bool wasPressed = false;
    public float speed = 2.0f;
    public int dir = -1;
    // Start is called before the first frame update
    void Start()
    {
        if (myImage == null)
        {
            myImage = GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myImage != null)
        {
            if (wasPressed == false)
            {
                if(dir == -1)
                {
                    myImage.color = Color.Lerp(myImage.color, Color.yellow, speed * Time.deltaTime);
                    if(myImage.color == Color.yellow)
                    {
                        dir = 1;
                    }
                }
                else
                {
                    myImage.color = Color.Lerp(myImage.color, Color.white, speed *  Time.deltaTime);
                    if (myImage.color == Color.white)
                    {
                        dir = -1;
                    }
                }
            }
        }
    }

    public void PressButton()
    {
        wasPressed = true;
        myImage.color = Color.white;
    }
}
