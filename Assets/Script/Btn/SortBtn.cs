using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortBtn : MonoBehaviour
{
    /// <summary>
    /// save load manager
    /// </summary>
    public SaveLoadManager m_saveLoadManager = null;

    /// <summary>
    /// down arrow img
    /// </summary>
    public Image m_dwnImg = null;

    /// <summary>
    /// up arrow img
    /// </summary>
    public Image m_upImg = null;

    /// <summary>
    /// type0 = save index sort
    /// type1 = name descending order
    /// type2 = name ascending oreder
    /// </summary>
    public int m_nowType = 0;

    /// <summary>
    /// if this btn click
    /// </summary>
    public void Click()
    {
        m_nowType++;
        if (m_nowType > 1) m_nowType = 0;
        m_saveLoadManager.m_nowSortType = m_nowType;

        if (m_nowType == 1)
        {
            m_saveLoadManager.SortType1();
            m_dwnImg.gameObject.SetActive(false);
            m_upImg.gameObject.SetActive(true);
        }
        else
        {
            m_saveLoadManager.SortType0();
            m_dwnImg.gameObject.SetActive(true);
            m_upImg.gameObject.SetActive(false);
        }
    }
}
