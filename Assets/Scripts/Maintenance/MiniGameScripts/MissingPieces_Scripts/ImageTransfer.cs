using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handle the image transfer when drag/drop
public class ImageTransfer : MonoBehaviour
{
    [SerializeField]MatchBox parentMatchBox;
    [SerializeField]MatchBox justCollidedMatchBox;
    ImageDrag imageDragComponent; // this component moves the images around as it collide with the boxes
    public int imageId;
    public int parentMatchBoxid;
    public bool isColliding = false;

    private void Start()
    {
        imageDragComponent = gameObject.GetComponent<ImageDrag>();
        if(imageDragComponent == null)
        {
            Debug.LogError("ImageDrag component needed in " + gameObject.name);
        }
    }

    private void Update()
    {
        CheckColliding();
    }

    private void CheckColliding()
    {
        if (!isColliding)
        {
            return; //if we do not collide with any boxes, do nothing
        }
        if (Input.GetMouseButtonUp(0)) //called when left mouse releases
        {
            if (justCollidedMatchBox.HasImage())
            {
                SwapImagesBetweenBoxes();
                isColliding = false;
            }
            else
            {
                ChangeCurrentMatchBox();
                isColliding = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "MatchBox") { return; }//if collide not with a match box do nothing
  
        justCollidedMatchBox = collision.gameObject.GetComponent<MatchBox>();
        if(justCollidedMatchBox == null)
        {
            Debug.LogError("Collision without matchbox component");
            return;
        }
        if(justCollidedMatchBox.GetBoxId() == parentMatchBoxid)
        { //if collide with the current box parent, do nothing
            return; 
        } 
        //else we start watching mouse movement to tranfser image
        isColliding = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }

    //if players want to transfer an image into a box that already have one, we swap the images betwen justCollidedMatchBox
    //and parentMatchBox
    private void SwapImagesBetweenBoxes()
    {
        //Debug.Log("Swap called");
        ImageTransfer otherImage = justCollidedMatchBox.GetChildImage(); 
        otherImage.UpdateParentMatchBox(parentMatchBox);
        UpdateParentMatchBox(justCollidedMatchBox);
    }

    //assuming we change the image parent to the JustCollidedMatchBox
    private void ChangeCurrentMatchBox()
    {
        //Debug.Log("ChangeCurrentMatchBox called");
        parentMatchBox.OnImageOut();
        UpdateParentMatchBox(justCollidedMatchBox);
    }

    public void UpdateParentMatchBox(MatchBox newParent)
    {
        parentMatchBox = newParent;
        parentMatchBoxid = newParent.GetBoxId();
        parentMatchBox.OnImageIn(this);
        gameObject.GetComponent<RectTransform>().position = newParent.GetPosition();
        imageDragComponent.SetNewPosition(newParent.GetPosition());
        //Debug.Log("Image " + imageId + " changed parent to " + parentMatchBoxid + " " + parentMatchBox.GetBoxId());
    }

    public void ResetState(int id, MatchBox newParent)
    {
        imageId = id;
        parentMatchBoxid = id;

        justCollidedMatchBox = newParent;
        ChangeCurrentMatchBox();
        isColliding = false;


    }

    public void SetJustCollidedMatchBox(MatchBox justCollided)
    {
        justCollidedMatchBox = justCollided;
    }
}
