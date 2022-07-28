using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameNotiManager : MonoBehaviour
{
    #region singleton
    private static InGameNotiManager _i;
    public static InGameNotiManager Instance
    {
        get
        {
            if (_i != null)
                return _i;

            _i = FindObjectOfType<InGameNotiManager>();

            if (_i != null)
                return _i;

            Create();

            return _i;
        }
    }
    public static InGameNotiManager Create()
    {
        GameObject TimerGameObject = new GameObject("IngameNoti");
        _i = TimerGameObject.AddComponent<InGameNotiManager>();
        return _i;
    }
    #endregion
    public const float timePerMsg = 5f; // 5 sec per message;
    const int showPerTime = 4; //show 4 message mostly per time;

    public TMPro.TMP_Text tmp_text;
    public struct IngameNoti
    {
        public string message;
        public GameObject source;
        public float time { get; set; }
        public IngameNoti(string s, GameObject go)
        {
            message = s;
            source = go;
            time = timePerMsg;
        }
    }

    Queue<IngameNoti> NotiQ = new Queue<IngameNoti>();
    List<IngameNoti> CurrNotiList = new List<IngameNoti>();

    // Start is called before the first frame update
    void Start()
    {
        //NewNotification(new IngameNoti("Your pet is hungry.", null));
    }

    // Update is called once per frame
    void Update()
    {
        bool gamePause = false; // get from time manager;

        if(!gamePause)
        {
            if (CurrNotiList.Count < showPerTime && NotiQ.Count>0)
                CurrNotiList.Add(NotiQ.Dequeue());

                ShowMessage();
        }
    }

    //show notification every 5 seconds 
    void ShowMessage()
    {
        string msg = "";
        for (int i = CurrNotiList.Count - 1; i > -1; i--)
        {
            IngameNoti temp = CurrNotiList[i];
            msg += temp.message + '\n';
            temp.time -= Time.deltaTime;
            if (CurrNotiList[i].time <= 0)
                CurrNotiList.RemoveAt(i);
            else
                CurrNotiList[i] = temp;
        }
        tmp_text.text = msg;
    }

    public void NewNotification(IngameNoti _noti)  
    {
        NotiQ.Enqueue(_noti);
        //Debug.Log("noti is called");
    }

    public void NewNotification_Plant(string _plantName, GameObject _gameObject)
    {
        NewNotification(new IngameNoti(_plantName + " is ready to harvest.", _gameObject));
    }

    public void NewNotification_Pet_Hungry(GameObject _pet)
    {
        NewNotification(new IngameNoti("Your pet is hungry.", _pet));
    }



}
