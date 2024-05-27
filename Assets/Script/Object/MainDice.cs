using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainDice : MonoBehaviour
{
    /// <summary>
    /// dice name
    /// </summary>
    public string m_name = string.Empty;

    /// <summary>
    /// dice type
    /// </summary>
    public int m_type = -1;

    /// <summary>
    /// own number
    /// </summary>
    public int m_number = -1;

    /// <summary>
    /// dice roll imfo
    /// </summary>
    public string[] m_diceImfo = null;

    /// <summary>
    /// now dice imfo index
    /// </summary>
    public int m_index = 0;

    /// <summary>
    /// name Text
    /// </summary>
    public Text m_nameText = null;

    /// <summary>
    /// text
    /// </summary>
    public Text m_text = null;

    /// <summary>
    /// click
    /// </summary>
    public void Click()
    {
        DiceImfoManager.Instance.GetDiceImfo(this);
    }

    /// <summary>
    /// reset dice texts
    /// </summary>
    public void SetText()
    {
        m_nameText.text = m_name;
        m_text.text = m_diceImfo[m_index];
    }

    /// <summary>
    /// get my imfo
    /// </summary>
    public string GetNowValue
    {
        get
        {
            return m_diceImfo[m_index];
        }
    }
}
