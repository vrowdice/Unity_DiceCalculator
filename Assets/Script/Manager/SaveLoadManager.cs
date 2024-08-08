using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [Header("Save Load Imformation")]

    /// <summary>
    /// save path
    /// </summary>
    [TextArea]
    public string m_saveFileName = string.Empty;

    /// <summary>
    /// save path
    /// </summary>
    [TextArea]
    public string m_groupSaveFileName = string.Empty;

    /// <summary>
    /// now saved dice
    /// </summary>
    public List<DiceData> m_saveNumDiceList = new List<DiceData>();

    /// <summary>
    /// now saved dice
    /// </summary>
    public List<DiceData> m_saveStrDiceList = new List<DiceData>();

    [Header("Save Load Panel")]

    /// <summary>
    /// now sort type
    /// </summary>
    public int m_nowSortType = 0;

    /// <summary>
    /// dice group name to del
    /// </summary>
    public string m_groupName = string.Empty;

    /// <summary>
    /// load dice panel prefab
    /// </summary>
    public GameObject m_loadDicePanel = null;

    /// <summary>
    /// load panel
    /// </summary>
    public GameObject m_loadPanel = null;

    /// <summary>
    /// this gameobj group will deactive when panel type 1
    /// </summary>
    public GameObject m_type0Group = null;

    /// <summary>
    /// load num panel content
    /// </summary>
    public GameObject m_numPanelContent = null;

    /// <summary>
    /// load str panel content
    /// </summary>
    public GameObject m_strPanelContent = null;

    /// <summary>
    /// load panel list
    /// </summary>
    public List<LoadPanel> m_numLoadPanelList = new List<LoadPanel>();

    /// <summary>
    /// load panel list
    /// </summary>
    public List<LoadPanel> m_strLoadPanelList = new List<LoadPanel>();

    /// <summary>
    /// now panel type
    /// 0 = dice
    /// 1 = dice group
    /// </summary>
    int m_panelType = 0;

    /// <summary>
    /// start
    /// </summary>
    private void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/" + m_saveFileName + ".json") ||
            !Directory.Exists(Application.persistentDataPath + "/" + m_groupSaveFileName)) ResetSave();

        m_panelType = -1;
    }

    /// <summary>
    /// data load
    /// </summary>
    /// <param name="argPath">data path</param>
    /// <returns>data string</returns>
    string Load(string argPath)
    {
        string _path = Application.persistentDataPath + "/" + argPath + ".json";
        string _data = string.Empty;
        StreamReader _sr = new StreamReader(_path, System.Text.Encoding.UTF8);
        _data = _sr.ReadToEnd();
        _sr.Close();

        return _data;
    }

    /// <summary>
    /// data save
    /// </summary>
    /// <param name="argPath">data path</param>
    /// <param name="argData">data string</param>
    void Save(string argPath, string argData)
    {
        string _path = Application.persistentDataPath + "/" + argPath + ".json";
        StreamWriter _sw = new StreamWriter(_path, false, System.Text.Encoding.UTF8);
        _sw.WriteLine(argData);
        _sw.Close();
    }

    /// <summary>
    /// save dice info
    /// </summary>
    public void SaveData()
    {
        SaveData _save = new SaveData();

        for(int i = 0; i < m_saveNumDiceList.Count; i++)
        {
            DiceData _dice = m_saveNumDiceList[i];

            _save.m_numDiceName.Add(_dice.m_name);
            _save.m_numDiceImfo.Add(_dice.m_diceImfo);
        }

        for (int i = 0; i < m_saveStrDiceList.Count; i++)
        {
            DiceData _dice = m_saveStrDiceList[i];

            _save.m_strDiceName.Add(_dice.m_name);
            _save.m_strDiceImfo.Add(_dice.m_diceImfo);
        }
        
        Save(m_saveFileName, _save.ToJsonString());
    }

    /// <summary>
    /// load dice info
    /// </summary>
    public bool LoadData()
    {
        SaveData _save = new SaveData(Load(m_saveFileName));
        if (_save == null) return false;

        m_saveNumDiceList = new List<DiceData>();
        for(int i = 0; i < _save.m_numDiceName.Count; i++)
        {
            DiceData _dice = new DiceData();
            _dice.m_name = _save.m_numDiceName[i];
            _dice.m_diceImfo = _save.m_numDiceImfo[i];
            _dice.m_type = 1;

            m_saveNumDiceList.Add(_dice);
        }

        m_saveStrDiceList = new List<DiceData>();
        for (int i = 0; i < _save.m_strDiceName.Count; i++)
        {
            DiceData _dice = new DiceData();
            _dice.m_name = _save.m_strDiceName[i];
            _dice.m_diceImfo = _save.m_strDiceImfo[i];
            _dice.m_type = 2;

            m_saveStrDiceList.Add(_dice);
        }
        
        return true;
    }

    /// <summary>
    /// save dice group data
    /// </summary>
    public void SaveGroupData(DiceGroupSaveData argData)
    {
        Save(m_groupSaveFileName + "/" + argData.m_diceGroupName, argData.ToJsonString());
    }

    /// <summary>
    /// load dice group data
    /// </summary>
    /// <param name="argIndex">group data name</param>
    public DiceGroupSaveData LoadGroupData(string argName)
    {
        DiceGroupSaveData _save = new DiceGroupSaveData(Load(m_groupSaveFileName + "/" + argName));
        if (_save == null)
        {
            return null;
        }

        return _save;
    }

    /// <summary>
    /// load all groups name data
    /// </summary>
    /// <returns>groups name</returns>
    public string[] LoadAllGroupsNameData()
    {
        DirectoryInfo _di = new DirectoryInfo(Application.persistentDataPath + "/" + m_groupSaveFileName);
        FileInfo[] _fileName = _di.GetFiles();
        string[] _str = new string[_fileName.Length];
        for(int i = 0; i < _fileName.Length; i++)
        {
            _str[i] = _fileName[i].Name;
            _str[i] = _str[i].Remove(_str[i].Length - 5);
        }
        return _str;
    }

    /// <summary>
    /// reset savefile
    /// </summary>
    public void ResetSave()
    {
        SaveData _save = new SaveData();

        Save(m_saveFileName, _save.ToJsonString());
        Directory.CreateDirectory(Application.persistentDataPath + "/" + m_groupSaveFileName);
    }

    /// <summary>
    /// open load panel
    /// </summary>
    public void SetDiceLoadPanel()
    {
        if (!LoadData()) return;

        m_type0Group.SetActive(true);
        m_panelType = 0;
        ResetLoadPanel();

        for (int i = 0; i < m_saveNumDiceList.Count; i++)
        {
            LoadPanel _panel = Instantiate(m_loadDicePanel).GetComponent<LoadPanel>();
            _panel.gameObject.transform.SetParent(m_numPanelContent.transform);
            _panel.gameObject.transform.localScale = new Vector3(1, 1, 1);

            _panel.m_index = i;
            _panel.m_type = 1;
            _panel.m_name.text = m_saveNumDiceList[i].m_name;

            m_numLoadPanelList.Add(_panel);
        }
        for (int i = 0; i < m_saveStrDiceList.Count; i++)
        {
            LoadPanel _panel = Instantiate(m_loadDicePanel).GetComponent<LoadPanel>();
            _panel.gameObject.transform.SetParent(m_strPanelContent.transform);
            _panel.gameObject.transform.localScale = new Vector3(1, 1, 1);

            _panel.m_index = i;
            _panel.m_type = 2;
            _panel.m_name.text = m_saveStrDiceList[i].m_name;

            m_strLoadPanelList.Add(_panel);
        }

        if (m_nowSortType == 0)
        {
            SortType0();
        }
        else if (m_nowSortType == 1)
        {
            SortType1();
        }

        m_loadPanel.SetActive(true);
    }

    public void SetDiceGroupLoadPanel()
    {
        StartCoroutine(IESetDiceGroupLoadPanel());
    }

    /// <summary>
    /// sort type 1
    /// descending order
    /// </summary>
    public void SortType0()
    {
        m_numLoadPanelList.Sort((a, b) => {
            return a.m_name.text.CompareTo(b.m_name.text);
        });
        m_strLoadPanelList.Sort((a, b) => {
            return a.m_name.text.CompareTo(b.m_name.text);
        });

        for (int i = 0; i < m_numLoadPanelList.Count; i++)
        {
            m_numLoadPanelList[i].gameObject.transform.SetAsLastSibling();
        }
        for (int i = 0; i < m_strLoadPanelList.Count; i++)
        {
            m_strLoadPanelList[i].gameObject.transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// sort type 2
    /// ascending order
    /// </summary>
    public void SortType1()
    {
        m_numLoadPanelList.Sort((a, b) => {
            return a.m_name.text.CompareTo(b.m_name.text);
        });
        m_strLoadPanelList.Sort((a, b) => {
            return a.m_name.text.CompareTo(b.m_name.text);
        });

        for (int i = 0; i < m_numLoadPanelList.Count; i++)
        {
            m_numLoadPanelList[i].gameObject.transform.SetAsFirstSibling();
        }
        for (int i = 0; i < m_strLoadPanelList.Count; i++)
        {
            m_strLoadPanelList[i].gameObject.transform.SetAsFirstSibling();
        }
    }

    /// <summary>
    /// click dice load btn
    /// </summary>
    /// <param name="argIndex"></param>
    public void DiceLoadBtn(int argIndex, int argType, string argName)
    {
        if(m_panelType == 0)
        {
            DiceImfoManager _imfoManager = DiceImfoManager.Instance;

            if (argType == 1)
            {
                _imfoManager.m_diceSidesInputField.text = (m_saveNumDiceList[argIndex].m_diceImfo.Length).ToString();
                _imfoManager.m_diceNameInputField.text = m_saveNumDiceList[argIndex].m_name;
                _imfoManager.m_diceTypeDropDown.value = m_saveNumDiceList[argIndex].m_type;
                _imfoManager.m_sampledice = m_saveNumDiceList[argIndex].m_diceImfo;
            }
            else
            {
                _imfoManager.m_diceSidesInputField.text = (m_saveStrDiceList[argIndex].m_diceImfo.Length).ToString();
                _imfoManager.m_diceNameInputField.text = m_saveStrDiceList[argIndex].m_name;
                _imfoManager.m_diceTypeDropDown.value = m_saveStrDiceList[argIndex].m_type;
                _imfoManager.m_sampledice = m_saveStrDiceList[argIndex].m_diceImfo;
            }
            _imfoManager.RefreshSample();

            m_loadPanel.SetActive(false);
            _imfoManager.m_diceImfoPanel.SetActive(true);
        }
        else if(m_panelType == 1)
        {
            StartCoroutine(GetDiceGroup(argName));
        }
    }

    /// <summary>
    /// click dice del btn
    /// </summary>
    /// <param name="argIndex"></param>
    public void DiceDel(int argIndex, int argType)
    {
        if (m_panelType == 0)
        {
            if (argType == 1)
            {
                m_saveNumDiceList.RemoveAt(argIndex);
                SaveData();
            }
            else
            {
                m_saveStrDiceList.RemoveAt(argIndex);
                SaveData();
            }

            SetDiceLoadPanel();
        }
        else if (m_panelType == 1)
        {
            if (m_groupName.Length <= 0)
            {
                ResetLoadPanelImfo();
                return;
            }

            File.Delete(Application.persistentDataPath + "/" + m_groupSaveFileName + "/" + m_groupName + ".json");
            for (int i = 0; i < m_numPanelContent.transform.childCount; i++)
            {
                if (m_numPanelContent.transform.GetChild(i).GetComponent<LoadPanel>().m_index == argIndex)
                {
                    Destroy(m_numPanelContent.transform.GetChild(i).gameObject);
                }
            }
            SetDiceGroupLoadPanel();
        }
    }

    /// <summary>
    /// reset loadpanel imfo and close
    /// </summary>
    void ResetLoadPanelImfo()
    {
        m_panelType = -1;
        m_groupName = string.Empty;
    }

    /// <summary>
    /// reset load panel
    /// </summary>
    void ResetLoadPanel()
    {
        for (int i = 0; i < m_numPanelContent.transform.childCount; i++)
        {
            Destroy(m_numPanelContent.transform.GetChild(i).gameObject);
            m_numLoadPanelList = new List<LoadPanel>();
        }
        for (int i = 0; i < m_strPanelContent.transform.childCount; i++)
        {
            Destroy(m_strPanelContent.transform.GetChild(i).gameObject);
            m_strLoadPanelList = new List<LoadPanel>();
        }
    }

    /// <summary>
    /// setting dice group load panel
    /// </summary>
    IEnumerator IESetDiceGroupLoadPanel()
    {
        string[] _str = LoadAllGroupsNameData();

        DiceManager.Instance.SetLoadingPanel(true);
        yield return new WaitForSeconds(1.0f);
        DiceManager.Instance.SetLoadingPanel(false);

        if (_str.Length <= 0)
        {
            DiceManager.Instance.Alert("No records saved");
            m_loadPanel.SetActive(false);

            yield break;
        }

        m_type0Group.SetActive(false);
        m_panelType = 1;
        ResetLoadPanel();

        for (int i = 0; i < _str.Length; i++)
        {
            LoadPanel _panel = Instantiate(m_loadDicePanel).GetComponent<LoadPanel>();
            _panel.gameObject.transform.SetParent(m_numPanelContent.transform);
            _panel.gameObject.transform.localScale = new Vector3(1, 1, 1);

            _panel.m_index = i;
            _panel.m_type = 0;
            _panel.m_name.text = _str[i];

            m_numLoadPanelList.Add(_panel);
        }

        if (m_nowSortType == 0)
        {
            SortType0();
        }
        else if (m_nowSortType == 1)
        {
            SortType1();
        }

        m_loadPanel.SetActive(true);
    }

    IEnumerator GetDiceGroup(string argName)
    {
        DiceGroupSaveData _data = LoadGroupData(argName);

        DiceManager.Instance.SetLoadingPanel(true);
        yield return new WaitForSeconds(1.0f);
        DiceManager.Instance.SetLoadingPanel(false);

        for (int i = 0; i < _data.m_numDiceName.Count; i++)
        {
            DiceManager.Instance.AddDice(1, _data.m_numDiceName[i], 1, _data.m_numDiceImfo[i]);
        }

        for (int i = 0; i < _data.m_strDiceName.Count; i++)
        {
            DiceManager.Instance.AddDice(1, _data.m_strDiceName[i], 2, _data.m_strDiceImfo[i]);
        }

        for (int i = 0; i < _data.m_operatorType.Count; i++)
        {
            DiceManager.Instance.ChangeOperatorType(i, _data.m_operatorType[i]);
        }
    }
}
