using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InteractionHandler handler = null;
    public GameObject temperatureButton = null;
    public Level levelControl = null;
    public TilePanel tilePanel = null;
    public PetPanel petPanel = null;
    [SerializeField] private bl_Joystick myJoystick;
    [SerializeField] private float mySpeed = 5;
    [SerializeField] float rotLerpSpeed = 2;

    [SerializeField]
    float h;

    [SerializeField]
    float v;

    [SerializeField] private AnimatorController myAnimator;


    public Transform moveTransform;
    public Transform rotationTransform;
    private Quaternion lastRotation;
    private Rigidbody myRigidbody;

    public List<GameObject> seedPool = new List<GameObject>();
    public bl_Joystick JOYSTICK
    {
        get { return myJoystick; }
    }
    public float SPEED
    {
        get { return mySpeed; }
        set { mySpeed = value; }
    }

    public Rigidbody RIGIDBODY
    {
        get { return myRigidbody; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (myJoystick == null)
        {
            myJoystick = GameObject.FindObjectOfType<bl_Joystick>();
        }
#if !UNITY_ANDROID || !UNITY_IOS
        myJoystick.gameObject.SetActive(false);
#endif
        if (myAnimator == null)
        {
            myAnimator = GetComponent<AnimatorController>();
        }

        if (myRigidbody == null)
        {
            myRigidbody = GetComponent<Rigidbody>();
        }
        if (myRigidbody == null)
        {
            myRigidbody = GetComponentInChildren<Rigidbody>();

        }
        if (handler == null)
        {
            handler = GameObject.FindObjectOfType<InteractionHandler>();
        }

        if (tilePanel == null)
            tilePanel = GameObject.FindObjectOfType<TilePanel>();

        if (petPanel == null)
            petPanel = GameObject.FindObjectOfType<PetPanel>();

        Application.targetFrameRate = 60;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        v = /*myJoystick.Vertical*/ Input.GetAxis("Vertical");
        h = /*myJoystick.Horizontal*/ Input.GetAxis("Horizontal");
#if UNITY_ANDROID
        v = myJoystick.Vertical;
        h = myJoystick.Horizontal;

#endif
        if (Mathf.Abs(v) < 0.1)
            v = 0;
        if (Mathf.Abs(h) < 0.1)
            h = 0;
        if (v == 0 && h == 0)
            myRigidbody.velocity = Vector3.zero;

        Vector3 translate = (new Vector3(h, 0, v));

        //if (myRigidbody != null)
        //{
        //    myRigidbody.MovePosition(Vector3.Lerp(myRigidbody.position, myRigidbody.position + translate, mySpeed * Time.fixedDeltaTime));
        //}
        //else
        //{
        //    if (moveTransform != null)
        //        moveTransform.position = Vector3.Lerp(moveTransform.position, moveTransform.position + translate, mySpeed * Time.fixedDeltaTime);
        //}

        if (myAnimator != null)
        {
            if (v == 0 && h == 0)
            {
                myAnimator.SetInt("animation,1");
            }
            else if (v != 0 || h != 0)
            {
                myRigidbody.velocity = translate * mySpeed; //Player's movement

                if (Mathf.Abs(v) > 0.1 || Mathf.Abs(h) > 0.1)
                {

                    if (handler != null)
                    {
                        if (handler.gameObject.activeInHierarchy)
                        {
                            handler.SetInactive();
                        }
                    }
                    if (temperatureButton != null)
                        temperatureButton.SetActive(false);
                    if (tilePanel != null)
                        tilePanel.setInactive();
                    if (petPanel != null)
                        petPanel.gameObject.SetActive(false);
                }
                myAnimator.SetInt("animation,18");
                if (rotationTransform != null)
                {
                    Vector3 lookVector = new Vector3(h, 0, v);
                    if (lookVector != Vector3.zero)
                    {

                        Quaternion newRoation = Quaternion.Lerp(rotationTransform.rotation, Quaternion.LookRotation(lookVector), Time.fixedDeltaTime * rotLerpSpeed);
                        rotationTransform.rotation = newRoation;
                    }
                }

            }


        }




    }

    public void GrantXP(int val)
    {

        if (levelControl != null)
        {
            levelControl.Exp(val);
        }
    }

}
