using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_R_Controller : MonoBehaviour
{

    Animator _RdoorAnim;

    private void OnTriggerEnter(Collider other)
    {
        _RdoorAnim.SetBool("isOpened", true);
    }

    private void OnTriggerExit(Collider other)
    {
        _RdoorAnim.SetBool("isOpened", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _RdoorAnim = this.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
