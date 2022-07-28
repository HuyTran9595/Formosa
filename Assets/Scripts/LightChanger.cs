using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : MonoBehaviour
{
    public List<Light> lights;
    public Color ori;
    public Color color1;
    public Color color2;
    public Color color3;
    // Start is called before the first frame update
    void Start()
    {
        ori = lights[0].color;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            foreach (var light in lights)
            {
                light.color = color1;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            foreach (var light in lights)
            {
                light.color = color2;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (var light in lights)
            {
                light.color = color3;
            }
        }
    }

    public void ChangeColor(int i)
    {
        switch (i)
        {
            case 1:
                foreach (var light in lights)
                {
                    light.color = color1;
                }
                break;
            case 2:
                foreach (var light in lights)
                {
                    light.color = color2;
                }
                break;
            case 3:
                foreach (var light in lights)
                {
                    light.color = color3;
                }
                break;
            default:
                break;
        }
    }

    [ContextMenu("GetAllLight")]
    void getAllLight()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            Light light = go.GetComponent<Light>();
            if (light) lights.Add(light);
        }
    }
}
