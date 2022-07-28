using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public Transform player;
    public float camYPosition = 58;//change this to fit the scene

    private void Start()
    {

        player = GameObject.Find("Boy Animation").transform;
        //this was for fixing bugs. Bug is fixed.
        //InvokeRepeating(nameof(PrintPositions), 2, 5);
    }
    private void Update()
    {
        if(player == null)
        {
            Debug.Log("Player is null minimap");
        }
        Vector3 newPosition = player.position;
        
        newPosition.y = camYPosition; //doesn't change the elevation of the minimap camera
        transform.position = newPosition;

    }

    private void PrintPositions()
    {
        Debug.Log("Player vector is: " + player.position.ToString());
        Debug.Log("Camera vector is: " + transform.position.ToString());

    }
}
