using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestTrackerUnloadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnload;
    }

    private void OnSceneUnload(Scene current)
    {
        Debug.Log("Unloaded scene: " + current.name);
        QuestTracker.ResetQuestTracker();
    }
}
