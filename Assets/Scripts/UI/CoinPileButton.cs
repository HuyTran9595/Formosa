using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPileButton : MonoBehaviour
{
    CoinPile pile;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ToggleOn(CoinPile coinPile)
    {
        pile = coinPile;
        gameObject.SetActive(true);
    }

    public void ToggleOff()
    {
        pile = null;
        gameObject.SetActive(false);
    }

    public void OnClicked()
    {
        pile.Interact();
    }
}
