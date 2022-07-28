using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadCreditScene()
    {
        SceneManager.LoadScene("Josh_Credits");
    }
}
