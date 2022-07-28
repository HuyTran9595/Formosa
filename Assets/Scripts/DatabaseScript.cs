using System;
using System.Collections.Generic;
using UnityEngine;
using TJayEnums;

public class DatabaseScript : MonoBehaviour
{
    public static TextAsset flowerData;
    public static TextAsset levelData;
    public static TextAsset petFoodData;

    public static Dictionary<int, string> flowerDataDictionary = new Dictionary<int, string>();
    public static Dictionary<int, string> levelDataDictionary = new Dictionary<int, string>();
    public static Dictionary<int, string> petFoodDataDictionary = new Dictionary<int, string>();
    public static List<ItemData> createdData = new List<ItemData>();
    public static List<int> createdDataNum = new List<int>();
    public static List<PetFood> createdPetFoodData = new List<PetFood>();
    public static List<int> createdPetFoodDataNum = new List<int>();


    public static ItemData GetItem(int index, ItemData.Tier tier = ItemData.Tier.None)
    {
        if (flowerData == null)
        {
            findFlowerData();
        }
        ItemData returnData = null;

        //Check all the created data, and see if any data fit the index and tier
        for (int i = 0; i < createdDataNum.Count; i++)
        {
            if (createdDataNum[i] == index)
            {
                ItemData data = createdData[i];
                if (data.tier == tier)
                    return data;
            }
        }

        //if not, create a new one
        string dataString = "";
        if (flowerDataDictionary.TryGetValue(index, out dataString))
        {
            string[] data = dataString.Split(',');
            ItemData.Type type = (ItemData.Type)Enum.Parse((typeof(ItemData.Type)), data[2]);
            switch (type)
            {
                case ItemData.Type.Seed:
                    returnData = ScriptableObject.CreateInstance<SeedData>();
                    break;
                case ItemData.Type.Plant:
                    returnData = ScriptableObject.CreateInstance<PlantData>();
                    break;
                case ItemData.Type.DryHerb:
                    returnData = ScriptableObject.CreateInstance<DryHerbData>();
                    break;
                case ItemData.Type.Potion:
                    returnData = ScriptableObject.CreateInstance<PotionData>();
                    break;
                case ItemData.Type.Recipe:
                    returnData = ScriptableObject.CreateInstance<RecipeData>();
                    break;
                case ItemData.Type.Task:
                    returnData = ScriptableObject.CreateInstance<TaskData>();
                    break;
                default:
                    returnData = ScriptableObject.CreateInstance<ItemData>();
                    break;
            }

            returnData.ItemType = type;
            returnData.ID = Int32.Parse(data[0]);
            returnData.ItemName = data[1];
            returnData.Price = Int32.Parse(data[6]);
            returnData.Description = data[7];
            returnData.Icon = Resources.Load<Sprite>("Images/" + data[8]);
            returnData.MaxHeld = 99;
            returnData.tier = tier;

            Biome possibleBiome = Biome.unknown;
            Genus possibleGenus = Genus.unknown;
            PotionType possiblePotion = PotionType.unknown;
            TaskType possibleTask = TaskType.buy;

            //0 - 99 Seeds Plants Herbs
            if (100 > index)
            {
                switch (returnData.ItemType)
                {
                    case ItemData.Type.Seed:
                        {
                            (returnData as SeedData).IdealTemperature = Int32.Parse(data[4]);
                            (returnData as SeedData).ProcessTime = (float)Double.Parse(data[3]);
                            (returnData as SeedData).ProductID = Int32.Parse(data[10]);
                            if (Enum.TryParse<Biome>(data[11], out possibleBiome))
                                (returnData as SeedData).biome = possibleBiome;
                            if (Enum.TryParse<Genus>(data[12], out possibleGenus))
                                (returnData as SeedData).genus = possibleGenus;
                            break;
                        }
                    case ItemData.Type.Plant:
                        {
                            (returnData as PlantData).ProcessTime = (float)Double.Parse(data[3]);
                            (returnData as PlantData).ProductID = Int32.Parse(data[10]);
                            if (Enum.TryParse<Biome>(data[11], out possibleBiome))
                                (returnData as PlantData).biome = possibleBiome;
                            if (Enum.TryParse<Genus>(data[12], out possibleGenus))
                                (returnData as PlantData).genus = possibleGenus;

                            if (returnData.tier == ItemData.Tier.Silver) returnData.Price = Mathf.CeilToInt((float)returnData.Price * ItemData.SilverBonus);
                            if (returnData.tier == ItemData.Tier.Gold) returnData.Price = Mathf.CeilToInt((float)returnData.Price * ItemData.GoldBonus);
                            break;
                        }
                    case ItemData.Type.DryHerb:
                        {
                            if (Enum.TryParse<Biome>(data[11], out possibleBiome))
                                (returnData as DryHerbData).biome = possibleBiome;
                            if (Enum.TryParse<Genus>(data[12], out possibleGenus))
                                (returnData as DryHerbData).genus = possibleGenus;

                            if (returnData.tier == ItemData.Tier.Silver) returnData.Price = Mathf.CeilToInt((float)returnData.Price * ItemData.SilverBonus);
                            if (returnData.tier == ItemData.Tier.Gold) returnData.Price = Mathf.CeilToInt((float)returnData.Price * ItemData.GoldBonus);
                            break;
                        }
                    default:
                        Debug.Log("Something wrong here");
                        break;
                }
            }
            //100 - 199 Recipes
            else if (200 > index)
            {
                (returnData as RecipeData).ProcessTime = (float)Double.Parse(data[3]);
                (returnData as RecipeData).FirstIngredient = Int32.Parse(data[4]);
                (returnData as RecipeData).SecondIngredient = Int32.Parse(data[5]);
                (returnData as RecipeData).ProductID = Int32.Parse(data[10]);

            }
            //200 - 299 Potions
            else if (300 > index)
            {
                (returnData as PotionData).Duration = (float)Double.Parse(data[3]);
                if (Enum.TryParse<PotionType>(data[11], out possiblePotion))
                    (returnData as PotionData).PotionType = possiblePotion;
                (returnData as PotionData).BoostSeedID = Int32.Parse(data[12]);
            }
            //1000 - 1099 Tasks
            else if (1100 > index)
            {
                (returnData as TaskData).UnlockNextTaskID = Int32.Parse(data[5]);
                if (Enum.TryParse<TaskType>(data[3], out possibleTask))
                    (returnData as TaskData).TaskType = possibleTask;
                (returnData as TaskData).IDForPlant = Int32.Parse(data[4]);
                (returnData as TaskData).ExpEarn = Int32.Parse(data[6]);
            }
            #region old code
            /*
            //id, item name,image, prefab, item Type, price, max held
            //ID, Name, Type, Process Time, Ideal Temp, unlock, Price, Description, Icon, Mesh,NextItemID



            returnData.ProcessTime = (float)Double.Parse(data[3]);
            returnData.IdealTemperature = Int32.Parse(data[4]);
            returnData.unlock = Int32.Parse(data[5]);


            // returnData.targetMesh = Resources.Load<Mesh>("Models/" + data[9]);
            returnData.EvolutionID = Int32.Parse(data[10]);
            // returnData.Model = data[3]; TODO get pre fab and add
            //Int32.Parse(data[6]);
            // returnData.biome = Int32.Parse(data[11]);
            //returnData.genus = Int32.Parse(data[12]);
            Biome possibleBiome = Biome.unknown;
            Genus possibleGenus = Genus.unknown;
            PotionType possiblePotion = PotionType.unknown;
            int otherNum = -1;
            if (Enum.TryParse<Biome>(data[11], out possibleBiome))
            {
                returnData.biome = (int)possibleBiome;
            }
            else if (Enum.TryParse<PotionType>(data[11], out possiblePotion))
            {
                returnData.biome = (int)possiblePotion;

            }


            if (Enum.TryParse<Genus>(data[12], out possibleGenus))
            {
                returnData.genus = (int)possibleGenus;
            }
            else if (Int32.TryParse(data[12], out otherNum))
            {
                returnData.genus = otherNum;
            }
            */
            #endregion
        }

        createdData.Add(returnData);
        createdDataNum.Add(index);
        return returnData;
    }

    public static void CheckLevelUpRewards(int index, Shop mainShop, Inventory playerInventory, ref List<LevelUpProp.newUnlock> unlocksList)
    {
        if (levelData == null)
        {
            findLevelData();
        }
        string dataString = "";
        if (levelDataDictionary.TryGetValue(index, out dataString))
        {
            string[] data = dataString.Split(',');
            int dataIndex = 1;
            for (int i = 0; i < data.Length; i += 2)
            {
                if (dataIndex + 1 < data.Length)
                {
                    UnlockType utype = UnlockType.task;
                    if (Enum.TryParse<UnlockType>(data[dataIndex], out utype))
                    {
                        int dataNum = 0;
                        dataIndex++;
                        if (Int32.TryParse(data[dataIndex], out dataNum))
                        {

                            switch (utype)
                            {
                                case UnlockType.item:
                                    {
                                        if (mainShop == null)
                                        {
                                            Debug.Log("mainShop null");
                                        }

                                        if (mainShop != null)
                                        {
                                            if (!mainShop.SoldItems.Contains(dataNum))
                                            {
                                                ItemData someItem = GetItem(dataNum);
                                                mainShop.items.Add(someItem);
                                                unlocksList.Add(new LevelUpProp.newUnlock(LevelUpProp.newUnlock.Type.Item, someItem.ItemName, someItem.Icon));
                                            }
                                        }
                                    }
                                    break;
                                case UnlockType.plot:
                                    {
                                        Debug.Log("Type is Plot In database");
                                        if (playerInventory.greenhouseManager != null)
                                        {
                                            playerInventory.greenhouseManager.UnlockNewPlot();
                                            unlocksList.Add(new LevelUpProp.newUnlock(LevelUpProp.newUnlock.Type.plot, ""));
                                        }
                                    }
                                    break;
                                case UnlockType.lab:
                                    {
                                        Debug.Log("Type is Lab In database");
                                        if (playerInventory.labManager != null)
                                        {
                                            playerInventory.labManager.UnlockNewPlot();
                                            unlocksList.Add(new LevelUpProp.newUnlock(LevelUpProp.newUnlock.Type.lab, ""));
                                        }
                                    }
                                    break;
                                case UnlockType.magic:
                                    {
                                        Debug.Log("Type is Magic In database");
                                        if (playerInventory.magicManager != null)
                                        {
                                            playerInventory.magicManager.UnlockNewPlot();
                                            unlocksList.Add(new LevelUpProp.newUnlock(LevelUpProp.newUnlock.Type.magic, ""));
                                        }
                                    }
                                    break;
                                case UnlockType.task:
                                    {
                                        if (playerInventory != null)
                                        {
                                            ItemData someItem = GetItem(dataNum);
                                            if (!playerInventory.taskObjectives.Contains(someItem))
                                            {
                                                playerInventory.taskObjectives.Add(someItem);
                                                if (playerInventory.station != null && playerInventory.station.playerJournal != null)
                                                    playerInventory.station.playerJournal.Pulse();
                                            }
                                        }
                                    }
                                    break;
                            }
                            // Debug.Log("Found " + utype + " " + dataNum);
                            dataIndex++;
                        }
                        else
                        {
                            Debug.Log("Parse failed");
                        }
                    }

                }
            }
        }
    }
    private static void loadFlowerData()
    {
        if (flowerData != null)
        {
            string fullString = flowerData.text;

            string[] rows = fullString.Split('\n');
            for (int i = 0; i < rows.Length; i++)
            {
                string rowData = rows[i];
                if (rowData.Length > 0)
                {
                    if (rowData[0] != '-')
                    {
                        int id = Int32.Parse(rowData.Split(',')[0]);
                        flowerDataDictionary.Add(id, rowData);
                    }
                }
            }
        }
    }
    private static void findFlowerData()
    {
        if (flowerData == null)
        {
            TextAsset possibleAsset = Resources.Load("FlowerData") as TextAsset;
            if (possibleAsset != null)
            {
                flowerData = possibleAsset;

            }
            else
            {
                TextAsset[] assets = Resources.FindObjectsOfTypeAll<TextAsset>();
                Debug.Log("found " + assets.Length + " assets ");
                for (int i = 0; i < assets.Length; i++)
                {
                    possibleAsset = assets[i];
                    if (possibleAsset.name == "FlowerData")
                    {
                        flowerData = possibleAsset;
                        break;
                    }
                }
            }
        }
        if (flowerData != null)
        {
            loadFlowerData();
        }
    }

    private static void loadLevelData()
    {
        if (levelData != null)
        {
            string fullString = levelData.text;

            string[] rows = fullString.Split('\n');
            for (int i = 0; i < rows.Length; i++)
            {
                string rowData = rows[i];
                if (rowData.Length > 0)
                {
                    if (rowData[0] != '-')
                    {
                        int id = Int32.Parse(rowData.Split(',')[0]);
                        levelDataDictionary.Add(id, rowData);
                    }
                }
            }
        }
    }
    private static void findLevelData()
    {
        if (levelData == null)
        {
            TextAsset possibleAsset = Resources.Load("LevelUnlock") as TextAsset;
            if (possibleAsset != null)
            {
                levelData = possibleAsset;

            }
            else
            {
                TextAsset[] assets = Resources.FindObjectsOfTypeAll<TextAsset>();
                Debug.Log("found " + assets.Length + " assets ");
                for (int i = 0; i < assets.Length; i++)
                {
                    possibleAsset = assets[i];
                    if (possibleAsset.name == "LevelUnlock")
                    {
                        levelData = possibleAsset;
                        break;
                    }
                }
            }
        }
        if (levelData != null)
        {
            loadLevelData();
        }
    }

    public static PetFood GetPetFood(int index)
    {
        if (petFoodData == null)
        {
            findpetFoodData();
        }
        PetFood returnData = null;
        if (createdPetFoodDataNum.Contains(index))
        {
            returnData = createdPetFoodData[createdPetFoodDataNum.IndexOf(index)];
        }
        else
        {
            returnData = ScriptableObject.CreateInstance<PetFood>();
        }

        string dataString = "";
        if (petFoodDataDictionary.TryGetValue(index, out dataString))
        {

            string[] data = dataString.Split(',');
            returnData.ID = Int32.Parse(data[0]);
            returnData.ItemName = data[1];
            returnData.Quality = (PetFood.PetFoodQuality)Enum.Parse((typeof(PetFood.PetFoodQuality)), data[2]);
            returnData.Price = Int32.Parse(data[3]);
            returnData.Duration = (float)Double.Parse(data[4]);
            returnData.Affection = Int32.Parse(data[5]);
            returnData.Icon = Resources.Load<Sprite>("Images/" + data[6].Trim());
            returnData.ItemType = ItemData.Type.PetFood;

        }
        if (!createdPetFoodData.Contains(returnData))
        {
            createdPetFoodData.Add(returnData);
            createdPetFoodDataNum.Add(index);
        }
        return returnData;
    }

    private static void loadpetFoodData()
    {
        if (petFoodData != null)
        {
            string fullString = petFoodData.text;

            string[] rows = fullString.Split('\n');
            for (int i = 0; i < rows.Length; i++)
            {
                string rowData = rows[i];
                if (rowData.Length > 0)
                {
                    if (rowData[0] != '-')
                    {
                        int id = Int32.Parse(rowData.Split(',')[0]);
                        petFoodDataDictionary.Add(id, rowData);
                    }
                }
            }
        }
    }
    private static void findpetFoodData()
    {
        if (petFoodData == null)
        {
            TextAsset possibleAsset = Resources.Load("PetFood") as TextAsset;
            if (possibleAsset != null)
            {
                petFoodData = possibleAsset;

            }
            else
            {
                TextAsset[] assets = Resources.FindObjectsOfTypeAll<TextAsset>();
                Debug.Log("found " + assets.Length + " assets ");
                for (int i = 0; i < assets.Length; i++)
                {
                    possibleAsset = assets[i];
                    if (possibleAsset.name == "LevelUnlock")
                    {
                        petFoodData = possibleAsset;
                        break;
                    }
                }
            }
        }
        if (petFoodData != null)
        {
            loadpetFoodData();
        }
    }

    public static bool GetItemType(int itemID, ref ItemData.Type type)
    {
        //If itemID is invalid, return false
        //else, true
        ItemData item = GetItem(itemID);
        if (item == null) return false;
        type = item.ItemType;
        return true;
    }
}
