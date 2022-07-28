using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_L_Controller : MonoBehaviour
{

    Animator _LdoorAnim;

    private void OnTriggerEnter(Collider other)
    {
        _LdoorAnim.SetBool("isOpened", true);
    }

    private void OnTriggerExit(Collider other)
    {
        _LdoorAnim.SetBool("isOpened", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _LdoorAnim = this.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
