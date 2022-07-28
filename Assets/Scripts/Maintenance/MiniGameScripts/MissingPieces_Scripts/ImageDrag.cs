using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// This class handle the drag and drop of the object
/// </summary>
public class ImageDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    //ImageTransfer imageTransferComponent;
    RectTransform rectTransform;
    Vector3 originalPosition;
    private Vector2 lastMousePosition;



    private void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        originalPosition = rectTransform.position;
        lastMousePosition = originalPosition;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        lastMousePosition = eventData.position;

    }

    /// <summary>
    /// drag the object according to the mouse position
    /// if the mouse is inside the screen
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;

        Vector3 newPosition = rectTransform.position + new Vector3(diff.x, diff.y, transform.position.z);
        Vector3 oldPos = rectTransform.position;
        rectTransform.position = newPosition;
        if (!IsRectTransformInsideSreen(rectTransform))
        {
            rectTransform.position = oldPos;
        }
        lastMousePosition = currentMousePosition;
    }

    /// <summary>
    /// snap back to old position when drag ends
    /// this doesn't affect the ImageTransfer methods because this get called before that
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.position = originalPosition;
        lastMousePosition = originalPosition;
    }


    /// <summary>
    /// This methods will check is the rect transform is inside the screen or not
    /// </summary>
    /// <param name="rectTransform">Rect Trasform</param>
    /// <returns></returns>
    private bool IsRectTransformInsideSreen(RectTransform rectTransform)
    {
        bool isInside = false;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        int visibleCorners = 0;
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        foreach (Vector3 corner in corners)
        {
            if (rect.Contains(corner))
            {
                visibleCorners++;
            }
        }
        if (visibleCorners == 4)
        {
            isInside = true;
        }
        return isInside;
    }

    //call this when changing image position on the panel
    public void SetNewPosition(Vector3 newPosition)
    {
        originalPosition = newPosition;
        lastMousePosition = originalPosition;
    }
}
