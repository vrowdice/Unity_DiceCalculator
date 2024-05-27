using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class DiceGroupSaveData
{
    /// <summary>
    /// this group data name
    /// </summary>
    public string m_diceGroupName = string.Empty;

    /// <summary>
    /// number dice name
    /// </summary>
    public List<string> m_numDiceName = new List<string>();

    /// <summary>
    /// number dice imfo
    /// </summary>
    public List<string[]> m_numDiceImfo = new List<string[]>();

    /// <summary>
    /// string dice name
    /// </summary>
    public List<string> m_strDiceName = new List<string>();

    /// <summary>
    /// string dice imfo
    /// </summary>
    public List<string[]> m_strDiceImfo = new List<string[]>();

    /// <summary>
    /// string dice imfo
    /// </summary>
    public List<int> m_operatorType = new List<int>();

    /// <summary>
    /// constructor
    /// </summary>
    public DiceGroupSaveData()
    {

    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="argData">string data</param>
    public DiceGroupSaveData(string argData)
    {
        DiceGroupSaveData _data = new DiceGroupSaveData();
        JsonConvert.PopulateObject(argData, _data);

        m_diceGroupName = _data.m_diceGroupName;

        m_numDiceName = _data.m_numDiceName;
        m_numDiceImfo = _data.m_numDiceImfo;

        m_strDiceName = _data.m_strDiceName;
        m_strDiceImfo = _data.m_strDiceImfo;

        m_operatorType = _data.m_operatorType;
    }

    /// <summary>
    /// string data to json
    /// </summary>
    /// <returns>string data</returns>
    public string ToJsonString()
    {
        string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
        return jsonString;
    }
}
