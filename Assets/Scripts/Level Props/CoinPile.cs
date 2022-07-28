using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPile : MonoBehaviour
{
    CoinPileManager cpm;
    GameObject UnpickedMesh, PickedMesh;
    bool IsPicked;
    CoinPileButton CoinPileButton;

    private void OnEnable()
    {
        cpm.AddPile(this);
        ChangeMesh();
    }
    private void OnDisable()
    {
        cpm.RemovePile(this);
    }
    // Start is called before the first frame update
    void Awake()
    {
        cpm = FindObjectOfType<CoinPileManager>();
        if (cpm == null)
            Debug.LogError("Coin Pile Cant Find Manager");
        CoinPileButton = FindObjectOfType<CoinPileButton>();
        if (CoinPileButton == null)
            Debug.LogError("Coin Pile Cant Find Button");


        UnpickedMesh = transform.GetChild(0).gameObject;
        PickedMesh = transform.GetChild(1).gameObject;
        ChangeMesh();
    }

    public void Interact()
    {
        cpm.CoinPilePicked(this);
        IsPicked = true;
        ChangeMesh();
        CoinPileButton.ToggleOff();
    }

    void ChangeMesh()
    {
        UnpickedMesh.SetActive(!IsPicked);
        PickedMesh.SetActive(IsPicked);
    }
    public void OnTriggerEnter(Collider Other)
    {
        if(!IsPicked)
            CoinPileButton.ToggleOn(this);
    }
    public void OnTriggerExit(Collider Other)
    {
        CoinPileButton.ToggleOff();
    }
    public void ChangeState(bool Picked)
    {
        IsPicked = Picked;
        ChangeMesh();
    }
}
