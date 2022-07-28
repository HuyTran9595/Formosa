using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Quest Tracker declares all the delegates that the player will invoke (aka all the things we track)
//Each individual quest will decide which delegate to subcribe to
//Player will invoke delegates when appropriate
public class QuestTracker : MonoBehaviour
{
    //every delegate needs 2 lines: declaration and initialization. 
    public delegate int OnPlayerLevelUp(int currentlevel); //invoked when player level up. Return int the number of level increased
    static public OnPlayerLevelUp LevelUp;//add subscription function to this var 


    public delegate int OnPlayerGatherSomething(int amount); //invoked when player gather Something


    static public OnPlayerGatherSomething ExampleQuest; //this is for QA


    static public OnPlayerGatherSomething BuyPlant; //buy any seed in the store
    static public OnPlayerGatherSomething BuyPotionRecipe;//buy recipe from store

    #region FLOWER_DELEGATES

    static public OnPlayerGatherSomething GrassRootSeed;
    static public OnPlayerGatherSomething GrassRoot;
    static public OnPlayerGatherSomething DriedGrassRoot;

    static public OnPlayerGatherSomething FragrantOrchidSeed;
    static public OnPlayerGatherSomething FragrantOrchid;
    static public OnPlayerGatherSomething DriedFragrantOrchid;

    static public OnPlayerGatherSomething DesertGrassRootSeed;
    static public OnPlayerGatherSomething DesertGrassRoot;
    static public OnPlayerGatherSomething DriedDesertGrassRoot;

    static public OnPlayerGatherSomething GiantJungleOrchidSeed;
    static public OnPlayerGatherSomething GiantJungleOrchid;
    static public OnPlayerGatherSomething DriedGiantJungleOrchid;

    static public OnPlayerGatherSomething ThornyJungleVineSeed;
    static public OnPlayerGatherSomething ThornyJungleVine;
    static public OnPlayerGatherSomething DriedThornyJungleVine;

    static public OnPlayerGatherSomething StrongForestHerbSeed;
    static public OnPlayerGatherSomething StrongForestHerb;
    static public OnPlayerGatherSomething DriedStrongForestHerb;

    static public OnPlayerGatherSomething GlowingOceanicFungiSeed;
    static public OnPlayerGatherSomething GlowingOceanicFungi;
    static public OnPlayerGatherSomething DriedGlowingOceanicFungi;

    static public OnPlayerGatherSomething PulsatingCaveMossSeed;
    static public OnPlayerGatherSomething PulsatingCaveMoss;
    static public OnPlayerGatherSomething DriedPulsatingCaveMoss;

    static public OnPlayerGatherSomething CarnivorousCavernVineSeed;
    static public OnPlayerGatherSomething CarnivorousCavernVine;
    static public OnPlayerGatherSomething DriedCarnivorousCavernVine;

    static public OnPlayerGatherSomething FieryDesertMossSeed;
    static public OnPlayerGatherSomething FieryDesertMoss;
    static public OnPlayerGatherSomething DriedFieryDesertMoss;

    //need a lot more delegates for flowers

    #endregion


    #region RECIPE_AND_ENHANCER_DELEGATE
    static public OnPlayerGatherSomething BuyGrassEnhancerRecipe;
    static public OnPlayerGatherSomething BuyFragrantEnhancerRecipe;
    static public OnPlayerGatherSomething BuyDesertGrassEnhancerRecipe;
    static public OnPlayerGatherSomething BuyGiantJungleRecipe;

    static public OnPlayerGatherSomething GatherGrassRootEnhancer;
    static public OnPlayerGatherSomething GatherFragrentEnhancer;
    static public OnPlayerGatherSomething GatherDesertGrassEnhancer;
    static public OnPlayerGatherSomething GatherGiantJungleOrchidEnhancer;

    #endregion


    //PLANT TIERS
    static public OnPlayerGatherSomething GoldTier;
    static public OnPlayerGatherSomething SilverTier;
    static public OnPlayerGatherSomething BronzeTier;

    //Planting temperature
    public delegate int OnPlayerPlanting(int temp);
    static public OnPlayerPlanting PlantingTemperature;

    //not finished
    #region PET_DELEGATES

    //BUY PETS
    static public OnPlayerGatherSomething BuyDog;
    static public OnPlayerGatherSomething BuyCat;
    static public OnPlayerGatherSomething BuyHorse;
    static public OnPlayerGatherSomething BuyFox;
    static public OnPlayerGatherSomething BuyTurtle;
    static public OnPlayerGatherSomething BuyRabit;

    //PETTING THE PET?



    //FEEDING THE PET?
    static public OnPlayerGatherSomething FeedAnyPet; //feed any pet X times
    static public OnPlayerGatherSomething FeedDog; //feed the dog X times
    static public OnPlayerGatherSomething FeedCat;
    static public OnPlayerGatherSomething FeedHorse;
    static public OnPlayerGatherSomething FeedFox;
    static public OnPlayerGatherSomething FeedTurtle;
    static public OnPlayerGatherSomething FeedRabbit;

    #endregion

    //BUYING AND SELLING
    public delegate int OnPlayerTransaction(int amount);
    //buying?
    
    //selling
    static public OnPlayerTransaction SellPlant;
    static public OnPlayerTransaction SellDryHerb;
    static public OnPlayerTransaction SellPotion;



    //Gather Coins
    static public OnPlayerGatherSomething EarnCoin; //amount of coin earned
    static public OnPlayerGatherSomething TotalCoin; //total amount of coins


    #region QUEST_SPECIFIC_MIX
    //quest 3: Fallen and can't get up
    static public OnPlayerGatherSomething SilverStrongForestHerb;

    //Quest 4: Who losses when elephants fight
    static public OnPlayerGatherSomething GoldGrassRoot;
    static public OnPlayerGatherSomething GoldDesertGrassRoot;
    #endregion

    //Quest 5: Fungi for a fun guy
    static public OnPlayerGatherSomething GoldGlowingOceanicFungi;
    static public OnPlayerGatherSomething SilverGlowingOceanicFungi;

    //Quest 6: What Could Go Wrong
    static public OnPlayerGatherSomething SilverThornyJungleVine;
    static public OnPlayerGatherSomething SilverCarnivorousCavernVine;
    static public OnPlayerGatherSomething SilverPulsatingCaveMoss;

    //Quest 8: A noble endeavor
    static public OnPlayerGatherSomething GoldFragrantOrchid;

    //Quest 11: A Souper Recovery
    static public OnPlayerGatherSomething GoldPulsatingCaveMoss;

    //Quest 12: From the Horses's Mouth
    static public OnPlayerGatherSomething SilverGrassRoot;
    static public OnPlayerGatherSomething SilverDesertGrassRoot;

    //Quest 13: Vine Inside the Jungle
    static public OnPlayerGatherSomething GoldGiantJungleOrchid;

    //Quest 14: Holy Guacamole
    static public OnPlayerGatherSomething GoldStrongForestHerb;
    static public OnPlayerGatherSomething SilverGiantJungleOrchid;

    //Quest 15: Common Scents
    static public OnPlayerGatherSomething SilverFragrantOrchid;

    //Quest 18: Fit For A King
    static public OnPlayerGatherSomething GoldCarnivorousCavernVine;

    //set all delegates to null
    public static void ResetQuestTracker()
    {
        LevelUp = null;

        ExampleQuest = null; //this is for QA


        BuyPlant = null; //buy any seed in the store
        BuyPotionRecipe = null;//buy recipe from store

        

        GrassRootSeed = null;
        GrassRoot = null;
        DriedGrassRoot = null;

        FragrantOrchidSeed = null;
        FragrantOrchid = null;
        DriedFragrantOrchid = null;

        DesertGrassRootSeed = null;
        DesertGrassRoot = null;
        DriedDesertGrassRoot = null;

        GiantJungleOrchidSeed = null;
        GiantJungleOrchid = null;
        DriedGiantJungleOrchid = null;

        ThornyJungleVineSeed = null;
        ThornyJungleVine = null;
        DriedThornyJungleVine = null;

        StrongForestHerbSeed = null;
        StrongForestHerb = null;
        DriedStrongForestHerb = null;

        GlowingOceanicFungiSeed = null;
        GlowingOceanicFungi = null;
        DriedGlowingOceanicFungi = null;

        PulsatingCaveMossSeed = null;
        PulsatingCaveMoss = null;
        DriedPulsatingCaveMoss = null;

        CarnivorousCavernVineSeed = null;
        CarnivorousCavernVine = null;
        DriedCarnivorousCavernVine = null;

        FieryDesertMossSeed = null;
        FieryDesertMoss = null;
        DriedFieryDesertMoss = null;

        //need a lot more delegates for flowers





        BuyGrassEnhancerRecipe = null;
        BuyFragrantEnhancerRecipe = null;
        BuyDesertGrassEnhancerRecipe = null;
        BuyGiantJungleRecipe = null;

        GatherGrassRootEnhancer = null;
        GatherFragrentEnhancer = null;
        GatherDesertGrassEnhancer = null;
        GatherGiantJungleOrchidEnhancer = null;


        //PLANT TIERS
        GoldTier = null;
        SilverTier = null;
        BronzeTier = null;


        PlantingTemperature = null;

        //not finished

        //BUY PETS
        BuyDog = null;
        BuyCat = null;
        BuyHorse = null;
        BuyFox = null;
        BuyTurtle = null;
        BuyRabit = null;

        //PETTING THE PET?



        //FEEDING THE PET?
        FeedAnyPet = null; //feed any pet X times
        FeedDog = null; //feed the dog X times
        FeedCat = null;
        FeedHorse = null;
        FeedFox = null;
        FeedTurtle = null;
        FeedRabbit = null;


    //selling
        SellPlant = null;
        SellDryHerb = null;
        SellPotion = null;



        //Gather Coins
        EarnCoin = null; //amount of coin earned
        TotalCoin = null; //total amount of coins



        //quest 3: Fallen and can't get up
        SilverStrongForestHerb = null;

        //Quest 4: Who losses when elephants fight
        GoldGrassRoot = null;
        GoldDesertGrassRoot = null;


        //Quest 5: Fungi for a fun guy
        GoldGlowingOceanicFungi = null;
        SilverGlowingOceanicFungi = null;

        //Quest 6: What Could Go Wrong
        SilverThornyJungleVine = null;
        SilverCarnivorousCavernVine = null;
        SilverPulsatingCaveMoss = null;

        //Quest 8: A noble endeavor
        GoldFragrantOrchid = null;

        //Quest 11: A Souper Recovery
        GoldPulsatingCaveMoss = null;

        //Quest 12: From the Horses's Mouth
        SilverGrassRoot = null;
        SilverDesertGrassRoot = null;

        //Quest 13: Vine Inside the Jungle
        GoldGiantJungleOrchid = null;

        //Quest 14: Holy Guacamole
        GoldStrongForestHerb = null;
        SilverGiantJungleOrchid = null;

        //Quest 15: Common Scents
        SilverFragrantOrchid = null;

        //Quest 18: Fit For A King
        GoldCarnivorousCavernVine = null;
    }
}
