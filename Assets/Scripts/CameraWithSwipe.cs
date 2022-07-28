using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWithSwipe : CameraParent
{
   

    private Vector3 currentMousePos;
    private Vector3 prevMousePos;
    private Vector3 oldMousePos;

    private bool rotate = false;
    Vector2 trueDirection;
    // Start is called before the first frame update
    void Start()
    {
        if (desiredDistance == Vector3.zero)
        {
            desiredDistance = new Vector3(0, 7, -5);
        }

        if (PlayerTarget == null)
        {
            PlayerTarget = GameObject.FindObjectOfType<PlayerController>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (PlayerTarget.JOYSTICK != null)
            {
                if (PlayerTarget.JOYSTICK.Vertical == 0 && PlayerTarget.JOYSTICK.Horizontal == 0)
                {
                    oldMousePos = prevMousePos;
                    prevMousePos = currentMousePos;
                    currentMousePos = Input.mousePosition;

                    if (prevMousePos != oldMousePos)
                    {
                        if (prevMousePos != currentMousePos)
                        {
                            if (currentMousePos != oldMousePos)
                            {

                                Vector3 direction = (currentMousePos - oldMousePos).normalized;
                                trueDirection = new Vector3(direction.y, direction.x);
                                rotate = true;
                            }
                        }
                    }
                }


            }

        }
        else if(Input.touchCount > 0)
        {
            if (PlayerTarget.JOYSTICK != null)
            {
                if (PlayerTarget.JOYSTICK.Vertical == 0 && PlayerTarget.JOYSTICK.Horizontal == 0)
                {
                    oldMousePos = prevMousePos;
                    prevMousePos = currentMousePos;
                    currentMousePos = Input.touches[0].position;

                    if (prevMousePos != oldMousePos)
                    {
                        if (prevMousePos != currentMousePos)
                        {
                            if (currentMousePos != oldMousePos)
                            {

                                Vector3 direction = (currentMousePos - oldMousePos).normalized;
                                trueDirection = new Vector3(direction.y, direction.x);
                                rotate = true;
                            }
                        }
                    }
                }


            }
        }
        else
        {
            rotate = false;
        }
    }

    private void LateUpdate()
    {
        Vector3 targetLocation = PlayerTarget.transform.position + desiredDistance;
        transform.position = Vector3.Lerp(transform.localPosition, targetLocation, lerpSpeed * Time.fixedDeltaTime);

        if (rotate == true)
        {
            transform.RotateAround(PlayerTarget.transform.position, trueDirection, 2 * Time.fixedDeltaTime);
        }
    }
}
