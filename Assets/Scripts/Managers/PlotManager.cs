using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    public List<Tiles> myTiles = new List<Tiles>();

    public void  UnlockNewPlot()
    {
        for (int i = 0; i < myTiles.Count; i++)
        {
            if(myTiles[i].gameObject.activeInHierarchy == false)
            {
                myTiles[i].gameObject.SetActive(true);
                break;
            }
        }
    }
}
