using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class MGWireBox : MonoBehaviour
{
    [SerializeField] 
    private string colorString;//Valid strings: blue, yellow, magenta, red. Everything else will print error in console


    private Image imageAttached; // the image attached to screen (for later, if decide to create arts)
    private bool matchStatus;


    void Start(){
        CheckNulls();
        InitializeVariables();
        CheckColorValidity(colorString);
        //Debug.Log("This image color is: " + imageAttached.color);
    }

    private void CheckNulls()
    {
        if (colorString == null)
        {
            Debug.LogError("Color string needed in " + gameObject.name);
        }
    }

    private void InitializeVariables()
    {
        imageAttached = gameObject.GetComponent<Image>();
        matchStatus = false;
    }



    //change the button color based on colorString value. Valid strings are: blue, yellow, magenta, red
    //print error if invalid colorString
    private bool CheckColorValidity(string colorString)
    {
        switch(colorString){
            case "red":
                //imageAttached.color = Color.red;
                return true;
            case "blue":
                //imageAttached.color = Color.blue;
                return true;
            case "yellow":
                //imageAttached.color = Color.yellow;
                return true;
            case "magenta":
                //imageAttached.color = Color.magenta;
                return true;
            default:
                Debug.LogError("Error wrong color string in "+ gameObject.name);
                return false;
        }
    }

    public string GetColorString(){
        return colorString;
    }
    
    public Color GetColor()
    {
        return imageAttached.color;
    }

    //set new colorString value if valid
    public bool SetColorString(string colorString){
        bool colorIsValid = CheckColorValidity(colorString);
        if(colorIsValid){
            this.colorString = colorString;
            return true;
        }
        return false;
    }

    public void SetMatchStatus(bool matchStatus){
        this.matchStatus = matchStatus;
    }

    public bool GetMatchStatus(){
        return matchStatus;
    }

    public void OnThisBoxClick(int boxId){
        Debug.Log("Box "+ boxId + " clicked.");
    }

    //call this when the game is reset
    public void ResetBox()
    {
        matchStatus = false;
    }

}
