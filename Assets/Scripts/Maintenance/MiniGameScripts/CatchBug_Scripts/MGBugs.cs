using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGBugs : MonoBehaviour
{
    bool isCatched = false;
    public void OnBugCatched(){
        isCatched = true;
        gameObject.SetActive(false);
    }
    public void Reset()
    {
        isCatched = false;
    }

}
