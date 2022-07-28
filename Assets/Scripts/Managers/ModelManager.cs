using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TJayEnums;

public class ModelManager : MonoBehaviour
{
    [System.Serializable]
    public struct GOArray
    {
        [Tooltip("Start, Medium, Finished")]
        public GameObject[] stageModels;
    }
    [Tooltip("Grass, Flower, Vine, Herb, Fungi, Moss")]
    public List<GOArray> models;

    [Header("Ignore the bottom part")]
    public GameObject grassStartModel;
    public GameObject grassMediumModel;
    public GameObject grassFinishedModel;

    public GameObject flowerStartModel;
    public GameObject flowerMediumModel;
    public GameObject flowerFinishedModel;

    public GameObject vineStartModel;
    public GameObject vineMediumModel;
    public GameObject vineFinishedModel;

    public GameObject herbStartModel;
    public GameObject herbMediumModel;
    public GameObject herbFinishedModel;

    public GameObject fungiStartModel;
    public GameObject fungiMediumModel;
    public GameObject fungiFinishedModel;

    public GameObject mossStartModel;
    public GameObject mossMediumModel;
    public GameObject mossFinishedModel;



    #region singleton
    private static ModelManager _instance;
    public static ModelManager Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = FindObjectOfType<ModelManager>();

            if (_instance != null)
                return _instance;

            Create();

            return _instance;
        }
    }
    private static ModelManager Create()
    {
        GameObject TimerGameObject = new GameObject("ModelManager");
        _instance = TimerGameObject.AddComponent<ModelManager>();
        return _instance;
    }

    #endregion
    private void Awake()
    {

        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
    }

    /*
    public Mesh GetBeginingMesh(Genus targetGenus)
    {
        Mesh convertingMesh = null;

        switch (targetGenus)
        {
            case Genus.unknown:
                break;
            case Genus.grass:
                if (grassStartModel != null)
                    convertingMesh = grassStartModel.GetComponent<MeshFilter>().sharedMesh;
                break;
            case Genus.flower:
                if (flowerStartModel != null)
                    convertingMesh = flowerStartModel.GetComponent<MeshFilter>().sharedMesh;

                break;
            case Genus.vine:
                if (vineStartModel != null)
                    convertingMesh = vineStartModel.GetComponent<MeshFilter>().sharedMesh;

                break;
            case Genus.herb:
                if (herbStartModel != null)
                    convertingMesh = herbStartModel.GetComponent<MeshFilter>().sharedMesh;

                break;
            case Genus.fungi:
                if (fungiStartModel != null)
                    convertingMesh = fungiStartModel.GetComponent<MeshFilter>().sharedMesh;

                break;
            case Genus.moss:
                if (mossStartModel != null)
                    convertingMesh = mossStartModel.GetComponent<MeshFilter>().sharedMesh;

                break;
        }

        return convertingMesh;
    }
  

    public Mesh GetMediumMesh(Genus targetGenus)
    {
        Mesh convertingMesh = null;

        switch (targetGenus)
        {
            case Genus.unknown:
                break;
            case Genus.grass:
                if (grassMediumModel != null)
                    convertingMesh = grassMediumModel.GetComponent<MeshFilter>().sharedMesh;
                break;
            case Genus.flower:
                if (flowerMediumModel != null)
                    convertingMesh = flowerMediumModel.GetComponent<MeshFilter>().sharedMesh;
                break;

            case Genus.vine:
                if (vineMediumModel != null)
                    convertingMesh = vineMediumModel.GetComponent<MeshFilter>().sharedMesh;
                break;

            case Genus.herb:
                if (herbMediumModel != null)
                    convertingMesh = herbMediumModel.GetComponent<MeshFilter>().sharedMesh;

                break;
            case Genus.fungi:
                if (fungiMediumModel != null)
                    convertingMesh = fungiMediumModel.GetComponent<MeshFilter>().sharedMesh;

                break;
            case Genus.moss:
                if (mossMediumModel != null)
                    convertingMesh = mossMediumModel.GetComponent<MeshFilter>().sharedMesh;

                break;
        }

        return convertingMesh;
    }

    public Mesh GetFinishedMesh(Genus targetGenus)
    {
        Mesh convertingMesh = null;

        switch (targetGenus)
        {
            case Genus.unknown:
                break;
            case Genus.grass:
                if (grassFinishedModel != null)
                    convertingMesh = grassFinishedModel.GetComponent<MeshFilter>().sharedMesh;
                break;
            case Genus.flower:
                if (flowerFinishedModel != null)
                    convertingMesh = flowerFinishedModel.GetComponent<MeshFilter>().sharedMesh;

                break;
            case Genus.vine:
                if (vineFinishedModel != null)
                    convertingMesh = vineFinishedModel.GetComponent<MeshFilter>().sharedMesh;

                break;
            case Genus.herb:
                if (herbFinishedModel != null)
                    convertingMesh = herbFinishedModel.GetComponent<MeshFilter>().sharedMesh;

                break;
            case Genus.fungi:
                if (fungiFinishedModel != null)
                    convertingMesh = fungiFinishedModel.GetComponent<MeshFilter>().sharedMesh;

                break;
            case Genus.moss:
                if (mossFinishedModel != null)
                    convertingMesh = mossFinishedModel.GetComponent<MeshFilter>().sharedMesh;

                break;
        }

        return convertingMesh;
    }
    */


    ///Get the model of genus with different stage
    ///Stage 0 = Begin, Stage 1 = Middle, Stage 2 = Finish
    public GameObject GetModelMesh(Genus targetGenus, int stage)
    {
        if(targetGenus == Genus.unknown)
        {
            Debug.LogError("Unknown Genus");
            return null;
        }
        if(stage > 3 || stage < 0)
        {
            Debug.LogError("What are you doing???");
            return null;
        }
        return models[(int)targetGenus].stageModels[stage];
    }
}
