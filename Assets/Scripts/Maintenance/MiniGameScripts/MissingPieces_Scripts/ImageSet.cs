using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class hold a set of sprites that represent an image 
public class ImageSet : MonoBehaviour
{
    static int NUMBER_OF_SPRITES_PER_IMAGES = 9;
    //the last sprite is the full correct image
    [SerializeField] Sprite[] imageList = new Sprite[NUMBER_OF_SPRITES_PER_IMAGES];
    [SerializeField] Sprite correctImage;


    public Sprite[] GetImageList()
    {
        return imageList;
    }

    public Sprite GetCorrectImage()
    {
        return correctImage;
    }

    public Sprite GetImage(int index)
    {
        if(index < 0 || index >= NUMBER_OF_SPRITES_PER_IMAGES)
        {
            Debug.LogError("Invalid index number");
            return null;
        }
        return imageList[index];
    }
}
