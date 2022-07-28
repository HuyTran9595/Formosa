using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWithAutoRotate : CameraParent
{
    public float deadZone = 0.2f;
    public Vector3 desiredEuler = new Vector3(0, 90, 0);

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

    }

    private void LateUpdate()
    {
        Vector3 desiredDirection = Quaternion.Euler(desiredEuler) * PlayerTarget.transform.forward;
        Vector3 targetLocation = Vector3.zero;
        if (PlayerTarget.RIGIDBODY != null)
        {
            
            targetLocation = PlayerTarget.RIGIDBODY.position + desiredDirection + desiredDistance;
        
            if ( Vector3.Distance(targetLocation, transform.position) > deadZone)
            {
                // transform.LookAt(PlayerTarget.transform);
                Quaternion lookRotation = Quaternion.LookRotation(PlayerTarget.transform.position - transform.position);
                transform.position = Vector3.Lerp(transform.position, targetLocation,  PlayerTarget.SPEED * Time.fixedDeltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, lerpSpeed * Time.fixedDeltaTime);
                transform.RotateAround(PlayerTarget.RIGIDBODY.position, PlayerTarget.transform.forward - transform.forward, lerpSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
          
            targetLocation = PlayerTarget.transform.position + desiredDirection + desiredDistance;
            transform.position = Vector3.Lerp(transform.localPosition, targetLocation, PlayerTarget.SPEED * Time.fixedDeltaTime);
            transform.RotateAround(PlayerTarget.transform.position, PlayerTarget.transform.forward - transform.forward, Time.fixedDeltaTime);
        }
    }
}
