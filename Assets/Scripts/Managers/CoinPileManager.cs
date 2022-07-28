using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPileManager : MonoBehaviour
{
    public enum algo { _1_RandomNumber, _2_Sequence }
    public algo Algorithm;
    public int min, max;
    public int[] sequence;
    int seq_index;

    Inventory Inventory;
    static List<CoinPile> piles = new List<CoinPile>();
    // Start is called before the first frame update
    void Start()
    {
        Inventory = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPile(CoinPile p) => piles.Add(p);
    public void RemovePile(CoinPile p) => piles.Remove(p);

    public void CoinPilePicked(CoinPile cp)
    {
        int amount = GetCoinAmount();
        Inventory.CoinUpdate(amount);
        string message = "";
        if (amount > 0)
            message = "You get " + amount.ToString() + " coins from coin pile!";
        else
            message = "You get nothing from the pile!";

        InGameNotiManager.Instance.NewNotification(new InGameNotiManager.IngameNoti(message, gameObject));
    }

    int GetCoinAmount()
    {
        switch (Algorithm)
        {
            case CoinPileManager.algo._1_RandomNumber:
                {
                    if(min > max)
                    {
                        int temp = min;
                        min = max;
                        max = temp;
                    }
                    return Random.Range(min, max+1);
                }
            case CoinPileManager.algo._2_Sequence:
                {
                    seq_index %= sequence.Length;
                    return sequence[seq_index++];
                }
            default:
                return 10;
        }
    }

    public void ResetAllPiles()
    {
        foreach (var pile in piles)
        {
            pile.ChangeState(false);
        }
    }
}
