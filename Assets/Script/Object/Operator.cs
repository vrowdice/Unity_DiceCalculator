using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Operator : MonoBehaviour
{
    /// <summary>
    /// this dropdown
    /// </summary>
    public Dropdown m_dropdown = null;

    /// <summary>
    /// operator type
    /// </summary>
    public int m_operatorType = 0;

    /// <summary>
    /// if change operator type
    /// </summary>
    public void ChangeOpeType(int argOpeType)
    {
        m_operatorType = argOpeType;
        m_dropdown.value = argOpeType;
    }
}
