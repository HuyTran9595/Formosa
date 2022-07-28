using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPotionRecipe : MonoBehaviour, SingleQuestInterface
{
    const int POTION_RECIPE = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerBuyPotionRecipe(int amount)
    {
        quest.UpdateQuestProgress(POTION_RECIPE, amount);
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.BuyPotionRecipe += PlayerBuyPotionRecipe;
    }

    public void Unsubscribe()
    {
        QuestTracker.BuyPotionRecipe -= PlayerBuyPotionRecipe;
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

