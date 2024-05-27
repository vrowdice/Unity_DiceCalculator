using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class SaveData
{
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
    /// constructor
    /// </summary>
    public SaveData()
    {

    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="argData">string data</param>
    public SaveData(string argData)
    {
        SaveData _data = new SaveData();
        JsonConvert.PopulateObject(argData, _data);

        m_numDiceName = _data.m_numDiceName;
        m_numDiceImfo = _data.m_numDiceImfo;

        m_strDiceName = _data.m_strDiceName;
        m_strDiceImfo = _data.m_strDiceImfo;
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
