using UnityEngine;

public class SaveLoadControl : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
            Save();
        if (Input.GetKeyDown(KeyCode.Keypad9))
            Load();
    }

    public void Save() => SaveLoadManager.Save();

    public void Load() => SaveLoadManager.Load();
}
