using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMovement : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 1.0f;
    public Vector3 startLocation;

    private void Awake()
    {
        transform.position = startLocation;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
