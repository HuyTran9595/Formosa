using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTile : MonoBehaviour
{
    public Magic_Shop MS_Script;
    void Start()
    {
        MS_Script = FindObjectOfType<Magic_Shop>();
    }
    public void Update()
    {

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != false)
        {
            if (MS_Script != null)
            {

                MS_Script.SetStore();
            }
        }
    }
    public void MouseOver()
    {
        Debug.Log("Test");
    }
    public void Clicked()
    {
        Debug.Log("Clicked");
        MS_Script.SetStore();
    }
}
