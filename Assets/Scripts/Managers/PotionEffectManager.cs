using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TJayEnums;
public class PotionEffectManager : MonoBehaviour
{
    #region singleton
    private static PotionEffectManager _instance;
    public static PotionEffectManager Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = FindObjectOfType<PotionEffectManager>();

            if (_instance != null)
                return _instance;

            Create();

            return _instance;
        }
    }
    private static PotionEffectManager Create()
    {
        GameObject TimerGameObject = new GameObject("PotionEffectManager");
        _instance = TimerGameObject.AddComponent<PotionEffectManager>();
        return _instance;
    }

    #endregion
    private void Awake()
    {

        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
    }
    public List<ItemData> ActivePotions = new List<ItemData>();
    public List<PotionType> ActiveEffects = new List<PotionType>();

    private void Update()
    {
        if (ActivePotions.Count > 0 && ActiveEffects.Count == ActivePotions.Count)
        {
            for (int i = ActivePotions.Count -1; i >= 0; i--)
            {
                PotionData runningPotion = ActivePotions[i] as PotionData;

                runningPotion.Duration -= Time.deltaTime;
                if(runningPotion.Duration <= 0)
                {
                    ActiveEffects.RemoveAt(i);
                    ActivePotions.RemoveAt(i);
                }
            }
        }
    }

    public int GetBoostSeedAmount(int TargetID)
    {
        for (int i = 0; i < ActivePotions.Count; i++)
        {
            PotionData data = ActivePotions[i] as PotionData;
            if (data.BoostSeedID == TargetID)
            {
                if (data.tier == ItemData.Tier.None) return 3;
                if (data.tier == ItemData.Tier.Tier1) return 4;
                if (data.tier == ItemData.Tier.Tier2) return 5;
                if (data.tier == ItemData.Tier.Tier3) return 6;
                if (data.tier == ItemData.Tier.Tier4) return 7;
            }
        }
        Debug.Log("Error");
        return 0;
    }
}
