using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchBox : MonoBehaviour
{
    [SerializeField] ImageTransfer currentChildImage;
    [SerializeField] ImageTransfer justCollidedImage;
    [SerializeField] int boxId; //unique to every box
    [SerializeField] int imageId = -1; // --1 means the match box is empty
    RectTransform rect;

    private void Start()
    {
        rect = gameObject.GetComponent<RectTransform>();
        if(currentChildImage != null)
        {
            imageId = currentChildImage.imageId;
        }
    }


    ////assuming collide with an image
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    justCollidedImage = collision.gameObject.GetComponent<ImageTransfer>();
    //    if(justCollidedImage == null)
    //    {
    //        Debug.LogError("MatchBox collided with objects without ImageTransfer component");
    //        return;
    //    }

    //    currentChildImage = justCollidedImage;
    //    imageId = currentChildImage.imageId;

    //}


    
    //the ImageTransfer component of MovingImage calls this class to update match box properly
    public void OnImageIn(ImageTransfer newChildImage)
    {
        currentChildImage = newChildImage;
        imageId = newChildImage.imageId;
       // Debug.Log("On Image In is called in Box " + boxId + " image id = " + imageId);
    }

    //the ImageTransfer component of MovingImage calls this class to update match box properly
    public void OnImageOut()
    {
        currentChildImage = null;
        imageId = -1;

        //Debug.Log("On Image Out is called in Box " + boxId + " image id = " + imageId);
    }



    //return rect transform position
    public Vector3 GetPosition()
    {
        return rect.position;
    }
    public Vector3 GetLocalPosition()
    {
        return rect.localPosition;
    }
    public bool HasImage()
    {
       if(imageId == -1)
       {
            return false;
       }
        Debug.Log("Has image, imageId = " + imageId);
        return true;

    }

    public int GetImageId()
    {
        return imageId;
    }

    public void SetImageId(int imageId)
    {
        this.imageId = imageId;
    }

    public int GetBoxId()
    {
        return boxId;
    }

    public ImageTransfer GetChildImage()
    {
        return currentChildImage;
    }
    
    public void ResetState(int id)
    {
        imageId = -1;
    }
}
