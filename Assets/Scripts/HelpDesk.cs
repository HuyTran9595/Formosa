using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpDesk : MonoBehaviour
{
    public GameObject helpDesk;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray;
        if (Input.touchCount > 0)
            ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
        else
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.tag == "Help Desk")
            {
                if (helpDesk.activeInHierarchy)
                    helpDesk.SetActive(true);
                else
                    helpDesk.SetActive(false);

            }
        }
    }
}
