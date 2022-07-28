using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDoor_Controller : MonoBehaviour
{

    Animator _BdoorAnim;

    private void OnTriggerEnter(Collider other)
    {
        _BdoorAnim.SetBool("isOpened", true);
    }

    private void OnTriggerExit(Collider other)
    {
        _BdoorAnim.SetBool("isOpened", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _BdoorAnim = this.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
