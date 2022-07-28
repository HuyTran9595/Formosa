using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProgressBar : MonoBehaviour
{
    [SerializeField] Transform barAnchor;


    public void SetSize(float sizeNormalized){
        if(sizeNormalized < 0){
            sizeNormalized = 0;
        }
        if(sizeNormalized > 1){
            sizeNormalized = 1;
        }
        barAnchor.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void Reset()
    {
        SetSize(0f);
    }
}
