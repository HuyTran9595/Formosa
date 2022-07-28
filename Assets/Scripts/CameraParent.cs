using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParent : MonoBehaviour
{
    public PlayerController PlayerTarget;
    public Vector3 desiredDistance = Vector3.zero;
    public float lerpSpeed = 1.0f;
   
}
