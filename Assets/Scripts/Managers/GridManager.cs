using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //inset prefab here
    public GameObject GridTilePrefab = null;

    [SerializeField]
    private Vector2 gridSize;

    private Vector2 previousSize;
    private List<GridTile> gridIndexs = new List<GridTile>();

    [SerializeField]//serialized for debugging
    private List<GridTile> indexPool = new List<GridTile>();


    public Vector2 GRID
    {
        get { return gridSize; }
        set
        {
            onGridChanged();
            previousSize = gridSize;
            gridSize = value;

        }
    }

    private void onGridChanged()
    {
        //Debug.Log("new value");
        Clear();
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.y; z++)
            {
                GridTile someTile = GetTile();
                if (someTile != null)
                {
                    gridIndexs.Add(someTile);

                    someTile.transform.localPosition = new Vector3(x, 0, z);
                }
            }
        }


    }

    //To update grid if a change is made with inspector
    private void OnValidate()
    {
        if (Application.isPlaying)
        {

            if (previousSize != gridSize)
            {
                Invoke("onGridChanged", 0.1f);
                //  onGridChanged(gridSize);

            }
        }
    }
    private void Clear()
    {
        for (int i = gridIndexs.Count - 1; i >= 0; i--)
        {
            GridTile current = gridIndexs[i];
            current.isInPool = true;
            indexPool.Add(current);
            gridIndexs.Remove(current);
            current.gameObject.SetActive(false);

        }

    }
    private GridTile GetTile()
    {
        GridTile someTile = null;

        for (int i = 0; i < indexPool.Count; i++)
        {
            GridTile current = indexPool[i];
            if (current.isInPool == true)
            {
                current.isInPool = false;
                indexPool.Remove(current);
                current.gameObject.SetActive(true);
                someTile = current;

                break;
            }
        }
        if (someTile == null)
        {
            if (GridTilePrefab == null)
            {
                Debug.LogError("No grid tile prefab found");
            }
            else
            {

                GameObject newTile = Instantiate(GridTilePrefab);
                if (!newTile.GetComponent<GridTile>())
                    newTile.AddComponent<GridTile>();

                someTile = newTile.GetComponent<GridTile>();
                someTile.isInPool = false;

            }
        }

        return someTile;
    }
}
