using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public bool isInPool = true;

    [SerializeField]
    private bool isOccupied = false;

    [SerializeField]
    private GridObject occupiedObject = null;

    public bool OCCUPIED
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

    public GridObject OBJECT
    {
        get { return occupiedObject; }
        set { occupiedObject = value; }
    }
}
