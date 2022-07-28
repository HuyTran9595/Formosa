using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridObject : MonoBehaviour
{
    [SerializeField]
    private int m_Width;

    [SerializeField]
    private int m_Height;

    public int WIDTH
    {
        get { return m_Width; }
        set { m_Width = value; }
    }

    public int HEIGHT
    {
        get { return m_Height; }
        set { m_Height = value; }
    }
}
