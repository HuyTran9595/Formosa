using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWithPresets : CameraParent
{
    public float desiredHeight = 7;
    public Vector3 preset1 = new Vector3(0, 0, -7); //0
    public Vector3 preset2 = new Vector3(-7, 90, 0); //90
    public Vector3 preset3 = new Vector3(0, 180, 7); // 180
    public Vector3 preset4 = new Vector3(7, 270, 0);
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Vector3 preset = GetPreset(1);
            SwitchToPreset(preset);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Vector3 preset = GetPreset(2);
            SwitchToPreset(preset);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Vector3 preset = GetPreset(3);
            SwitchToPreset(preset);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Vector3 preset = GetPreset(4);
            SwitchToPreset(preset);
        }
    }

    private Vector3 GetPreset(int presetNumber)
    {
        if (presetNumber == 1)
            return preset1;

        if (presetNumber == 2)
            return preset2;

        if (presetNumber == 3)
            return preset3;

        if (presetNumber == 4)
            return preset4;

        return preset1;
    }

    private void SwitchToPreset(Vector3 ChoosenPreset)
    {
        desiredDistance = new Vector3(ChoosenPreset.x, desiredHeight, ChoosenPreset.z);

        transform.eulerAngles = new Vector3(32.0f, ChoosenPreset.y, 0);
    }

    private void LateUpdate()
    {
        Vector3 targetLocation = PlayerTarget.transform.position + desiredDistance;
        transform.position = Vector3.Lerp(transform.localPosition, targetLocation, lerpSpeed * Time.fixedDeltaTime);
    }
}
