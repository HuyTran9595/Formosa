using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformUpdate : MonoBehaviour
{
    private Transform bodyAnimation;
    private void Start()
    {
        bodyAnimation = transform.Find("Boy Animation").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = bodyAnimation.position;
        newPosition.y = transform.position.y; //doesn't change the elevation of the minimap camera
        transform.position = newPosition;
    }
}
