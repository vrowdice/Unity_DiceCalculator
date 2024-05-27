using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSampleImg : MonoBehaviour
{
    /// <summary>
    /// input field
    /// </summary>
    public InputField m_inputfield = null;

    /// <summary>
    /// sample dice order
    /// </summary>
    public int m_number = -1;

    /// <summary>
    /// refresh sample dice array
    /// </summary>
    public void Refresh()
    {
        if (m_number != -1)
        {
            DiceImfoManager.Instance.m_sampledice[m_number] = m_inputfield.text;
        }
    }
}
