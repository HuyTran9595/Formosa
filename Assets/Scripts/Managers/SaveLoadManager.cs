using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class SaveLoadManager : MonoBehaviour
{
    //#region singleton
    //private static SaveLoadManager _i;
    //public static SaveLoadManager Instance
    //{
    //    get
    //    {
    //        if (_i != null)
    //            return _i;

    //        _i = FindObjectOfType<SaveLoadManager>();

    //        if (_i != null)
    //            return _i;

    //        Create();

    //        return _i;
    //    }
    //}
    //public static SaveLoadManager Create()
    //{
    //    GameObject TimerGameObject = new GameObject("SaveLoad");
    //    _i = TimerGameObject.AddComponent<SaveLoadManager>();
    //    return _i;
    //}
    //#endregion

    static bool saving = true;
    static bool loading = true;

    static List<GameObject> registedObject = new List<GameObject>();
    public static void Save()
    {
        Debug.Log("Saving...");
        //Split all Registed gameobject to different class
        List<Plant> plants = new List<Plant>();
        List<Tiles> tiles = new List<Tiles>();
        foreach (var item in registedObject)
        {
            Plant tplant = item.GetComponent<Plant>();
            if (tplant != null)
                plants.Add(tplant);
            //else if (ttile != null)
            //    tiles.Add(ttile);
            else
                Debug.Log("Save Manager cannot save this: " + item.name);
        }

        PlotManager[] pms = //new List<PlotManager>();
        MonoBehaviour.FindObjectsOfType<PlotManager>();
        foreach (var item in pms)
        {
            foreach (var tile in item.myTiles)
            {
                tiles.Add(tile);
            }
        }

        string _output = "{\n";

        //Get Tile Amount;
        _output += string.Format("\"TileSize\" : {0},\n", tiles.Count);
        if (tiles.Count > 0)
            _output += "\"Tiles\":[\n";
        //Save Tiles
        for (int i = 0; i < tiles.Count; i++)
        {
            _output += TileToJson(tiles[i]);
            if (i < tiles.Count - 1) _output += ",\n";
        }
        if (tiles.Count > 0)
            _output += "\n]\n";

        //Get Plant Amount
        _output += string.Format("\n\"PlantSize\" : {0},\n\n", plants.Count);
        //Save all plants
        if (plants.Count > 0)
            _output += "\"Plants\":[\n";

        for (int i = 0; i < plants.Count; i++)
        {
            string temp = PlantToJson(plants[i]);
            //string temp = JsonUtility.ToJson(testList[i], true);
            _output += temp;
            if (i < plants.Count - 1) _output += ",\n";
        }

        if (plants.Count > 0)
            _output += "\n]\n";


        //Get inventory
        Inventory inventory = GameObject.FindObjectOfType<Inventory>();
        if (inventory == null)
            Debug.LogError("Save Manager Can't find Inventory!");

        //Get All item
        List<ItemData> datas = new List<ItemData>();

        for (int i = 0; i < inventory.itemDatas.Length; i++)
        {
            datas.AddRange(inventory.itemDatas[i]);
        }

        _output += string.Format("\"InvSize\" : {0},\n", datas.Count);
        if (datas.Count > 0) _output += "\"Inv\":[\n";
        for (int i = 0; i < datas.Count; i++)
        {
            _output += ItemToJson(datas[i]);
            if (i < datas.Count - 1) _output += ",\n";
        }

        if (datas.Count > 0) _output += "\n]\n";


        //Get player Position
        Vector3 pos = inventory.transform.position;
        _output += string.Format("\n\"Pos\" : [{0}, {1}, {2}],\n", pos.x, pos.y, pos.z);

        _output += string.Format("\n\"Coin\" : {0},\n", inventory.Coins);







        //Done saving string
        _output += "}";
        File.WriteAllText(Application.dataPath + "/save.json", _output);
        Debug.Log("Gave Saved!");
    }
    public static void Load()
    {
        Debug.Log("Loading...");
        string filePath = Application.dataPath + "/save.json";
        if (File.Exists(filePath))
        {
            //Get the save file
            string output = File.ReadAllText(filePath);
            var json = JSON.Parse(output);

            //Find all game Tiles
            int tileSize = json["TileSize"];
            SortedList<int, Tiles> slTiles = new SortedList<int, Tiles>();
            PlotManager[] pms = MonoBehaviour.FindObjectsOfType<PlotManager>();
            foreach (var item in pms)
            {
                foreach (var tile in item.myTiles)
                {
                    if (!slTiles.ContainsKey(tile.ID))
                        slTiles.Add(tile.ID, tile);
                }
            }

            //Tile
            for (int i = 0; i < tileSize; i++)
            {
                slTiles[json["Tiles"][i]["TileID"]].CurrentTemperature = json["Tiles"][i]["TileTemp"];
            }

            //Plant
            int plantSize = json["PlantSize"];
            for (int i = 0; i < plantSize; i++)
            {
                Plant plant = new Plant();
                plant.harvestTime = json["Plants"][i]["HarvestTime"];
                plant.plantID = json["Plants"][i]["PlantID"];
                plant.Crop = DatabaseScript.GetItem(json["Plants"][i]["ItemData"]);
                plant.timePassed = json["Plants"][i]["RemainTime"];
                slTiles[json["Plants"][i]["CurrentTileID"]].LoadPlant(plant);
            }

            //Inventory
            Inventory inventory = GameObject.FindObjectOfType<Inventory>();
            Vector3 pos = new Vector3(json["Pos"][0], json["Pos"][1], json["Pos"][2]);
            inventory.transform.position = pos;
            inventory.Coins = json["Coin"];
            inventory.ClearInventory();
            int itemSize = json["InvSize"];

            for (int i = 0; i < itemSize; i++)
            {
                int itemID  = json["Inv"][i]["ItemID"];
                int amount  = json["Inv"][i]["CurrHeld"];
                int tier    = json["Inv"][i]["Tier"];
                ItemData item = DatabaseScript.GetItem(itemID, (ItemData.Tier)tier);
                inventory.AddItem(item, amount);
            }
            Debug.Log("Gave Loaded!");
        }
        else
            Debug.LogError("File doesn't exist!");
    }

    ///Register this object to be saved
    public static void Register(GameObject gameObject)
    {
        if (registedObject == null)
            registedObject = new List<GameObject>();
        if (!registedObject.Contains(gameObject))
            registedObject.Add(gameObject);
    }

    public static void UnRegister(GameObject gameObject)
    {
        if (registedObject == null)
            registedObject = new List<GameObject>();
        if (!registedObject.Contains(gameObject))
            registedObject.Remove(gameObject);
    }

    static string PlantToJson(Plant plant)
    {
        string output = "{" + String.Format(" " +
            "\"PlantID\" : {0}," +
            "\"HarvestTime\" : {1}," +
            "\"CropData\" : {2}," +
            "\"RemainTime\" : {3}," +
            "\"CurrentTileID\" : {4}" +
            "", plant.plantID, plant.harvestTime, plant.Crop.ID, plant.timePassed, plant.currentTile.ID) + "}";

        return output;
    }

    static string TileToJson(Tiles tile)
    {
        string output = "{" + String.Format(" " +
            "\"TileID\" : {0}," +
            "\"TileTemp\" : {1}", tile.ID, tile.CurrentTemperature) + "}";
        return output;
    }
    static string ItemToJson(ItemData item)
    {
        string output = "{" + String.Format(" " +
            "\"ItemID\" : {0}," +
            "\"CurrHeld\" : {1}," +
            "\"Tier\": {2}", item.ID, item.CurrHeld, (int)item.tier) + "}";
        return output;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}


