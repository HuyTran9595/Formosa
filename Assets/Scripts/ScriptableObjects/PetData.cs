using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Pet", menuName = "Pet Data")]
public class PetData : ItemData
{
    public float FindTimer;
    public List<int> Findable;
    public GameObject Model;

    public override void Copy(out ItemData des)
    {
        base.Copy(out des);
        if (des is PetData)
        {
            (des as PetData).FindTimer = this.FindTimer;
            (des as PetData).Findable = this.Findable;
            (des as PetData).Model = this.Model;
        }
    }
}
