using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingPosition : MonoBehaviour
{
    [SerializeField]RectTransform background;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<RectTransform>().position = background.position;
    }

}
