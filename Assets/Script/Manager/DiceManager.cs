using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    /// <summary>
    /// glober diceManager
    /// </summary>
    static DiceManager g_diceManager = null;

    /// <summary>
    /// save load manager
    /// </summary>
    public SaveLoadManager m_saveLoadManager = null;
    
    /// <summary>
    /// re ask
    /// </summary>
    public ReAskDelegate m_reAsk;

    /// <summary>
    /// re ask delegate
    /// </summary>
    public delegate void ReAskDelegate(int arg0, int arg1);

    int m_delArg0 = 0;

    int m_delArg1 = 0;

    [Header("Dice")]
    /// <summary>
    /// main dice prefeb obj
    /// </summary>
    public GameObject m_mainDice = null;

    /// <summary>
    /// main dice group prefeb obj
    /// </summary>
    public GameObject m_diceGroup = null;

    /// <summary>
    /// dice content gameobj
    /// </summary>
    public GameObject m_diceContent = null;

    /// <summary>
    /// no calculate dice content gameobj
    /// </summary>
    public GameObject m_noCalDiceContent = null;

    /// <summary>
    /// roll info panel
    /// </summary>
    public Text m_numRollInfoText = null;

    /// <summary>
    /// roll info panel
    /// </summary>
    public Text m_strRollInfoText = null;

    /// <summary>
    /// dice group list
    /// </summary>
    List<DiceGroup> m_numDiceGroupList = new List<DiceGroup>();

    /// <summary>
    /// no calculate dice obj list
    /// </summary>
    List<MainDice> m_strDiceList = new List<MainDice>();

    [Header("Dice Group")]

    /// <summary>
    /// group name input field
    /// </summary>
    public InputField m_groupNameInputField = null;

    /// <summary>
    /// group name input field
    /// </summary>
    public GameObject m_GroupNamePanel = null;

    [Header("Setting")]

    /// <summary>
    /// only result toggle
    /// </summary>
    public Toggle m_onlyResultToggle = null;

    /// <summary>
    /// no space toggle
    /// </summary>
    public Toggle m_noSpaceToggle = null;

    /// <summary>
    /// no operator toggle
    /// </summary>
    public Toggle m_noOperatorToggle = null;

    [Header("Log")]
    /// <summary>
    /// log content
    /// </summary>
    public GameObject m_logContent = null;

    /// <summary>
    /// log prefeb object
    /// </summary>
    public Text m_logObj = null;

    /// <summary>
    /// new log object
    /// </summary>
    Text m_nowLogObj = null;

    [Header("Alert")]
    /// <summary>
    /// re ask panel
    /// </summary>
    public GameObject m_reAskPanel = null;

    /// <summary>
    /// loading panel
    /// </summary>
    public GameObject m_loadingPanel = null;

    /// <summary>
    /// alert panel
    /// </summary>
    public GameObject m_alertPanel = null;

    /// <summary>
    /// alert Text
    /// </summary>
    public Text m_alertText = null;

    // Start is called before the first frame update
    void Start()
    {
        g_diceManager = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetReAsk(QuitApp, 0, 0);
        }
    }

    /// <summary>
    /// quit application
    /// </summary>
    /// <param name="arg0">none</param>
    /// <param name="arg1">none</param>
    void QuitApp(int arg0, int arg1)
    {
        Debug.Log("in");

        Application.Quit();
    }

    /// <summary>
    /// roll numdice
    /// </summary>
    bool RollNumDice()
    {
        if (m_numDiceGroupList.Count <= 0)
        {
            return false;
        }

        m_numRollInfoText.text = string.Empty;

        for (int i = 0; i < m_numDiceGroupList.Count; i++)
        {
            MainDice _dice = m_numDiceGroupList[i].m_mainDice;

            _dice.m_index = UnityEngine.Random.Range(0, _dice.m_diceImfo.Length);
            _dice.SetText();
        }

        double _dbl = double.Parse(m_numDiceGroupList[0].m_mainDice.GetNowValue);
        List<int> _newCal = new List<int>();
        string _str = string.Empty;
        if (m_noOperatorToggle.isOn)
        {
            _str = m_numDiceGroupList[0].m_mainDice.GetNowValue + " ";
        }
        else
        {
            _str = m_numDiceGroupList[0].m_mainDice.GetNowValue + " " + GetOpe(m_numDiceGroupList[0].m_operator.m_operatorType) + " ";
        }

        for (int i = 0; i < m_numDiceGroupList.Count - 1; i++)
        {
            string _tmpDblStr = m_numDiceGroupList[i + 1].m_mainDice.GetNowValue.ToString();

            switch (m_numDiceGroupList[i].m_operator.m_operatorType)
            {
                case 0:
                    _dbl += double.Parse(_tmpDblStr);
                    break;
                case 1:
                    _dbl -= double.Parse(_tmpDblStr);
                    break;
                case 2:
                    _dbl *= double.Parse(_tmpDblStr);
                    break;
                case 3:
                    _dbl /= double.Parse(_tmpDblStr);
                    break;
                default:
                    break;
            }

            if (m_noOperatorToggle.isOn)
            {
                _str += _tmpDblStr + " ";
            }
            else
            {
                _str += _tmpDblStr + " " + GetOpe(m_numDiceGroupList[i + 1].m_operator.m_operatorType) + " ";
            }
        }

        if (m_onlyResultToggle.isOn)
        {
            m_numRollInfoText.text = _dbl.ToString();

            m_nowLogObj.text += m_numRollInfoText.text;
            return true;
        }
        if (m_noOperatorToggle.isOn)
        {
            m_numRollInfoText.text = _str + _dbl.ToString();
        }
        else
        {
            m_numRollInfoText.text = _str.Remove(_str.Length - 3) + " = " + _dbl.ToString();
        }
        if (m_noSpaceToggle.isOn)
        {
            string _tmpStr = string.Empty;
            _tmpStr = m_numRollInfoText.text.Replace(" ", "");
            m_numRollInfoText.text = _tmpStr;
        }

        m_nowLogObj.text += m_numRollInfoText.text;

        return true;
    }

    /// <summary>
    /// roll no calculate dice
    /// </summary>
    bool RollStrDice()
    {
        if (m_strDiceList.Count <= 0)
        {
            return false;
        }

        m_strRollInfoText.text = string.Empty;

        string _str = string.Empty;
        for (int i = 0; i < m_strDiceList.Count; i++)
        {
            MainDice _dice = m_strDiceList[i];

            _dice.m_index = UnityEngine.Random.Range(0, _dice.m_diceImfo.Length);
            _dice.SetText();

            _str += _dice.GetNowValue + " ";
        }

        m_strRollInfoText.text = _str;
        if (m_noSpaceToggle.isOn)
        {
            string _tmpStr = string.Empty;
            _tmpStr = m_strRollInfoText.text.Replace(" ", "");
            m_strRollInfoText.text = _tmpStr;
        }

        m_nowLogObj.text += m_strRollInfoText.text;

        return true;
    }

    /// <summary>
    /// sand index and get operator
    /// </summary>
    /// <param name="argType">operator type</param>
    /// <returns></returns>
    string GetOpe(int argType)
    {
        string _str = "";

        switch (argType)
        {
            case 0:
                _str = "+";
                return _str;
            case 1:
                _str = "-";
                return _str;
            case 2:
                _str = "*";
                return _str;
            case 3:
                _str = "/";
                return _str;
            default:
                return _str;
        }
    }

    /// <summary>
    /// roll the dice
    /// </summary>
    public void RollDice()
    {
        m_nowLogObj = Instantiate(m_logObj);
        m_nowLogObj.transform.SetParent(m_logContent.transform);
        m_nowLogObj.transform.localScale = new Vector3(1, 1, 1);
        m_nowLogObj.text = DateTime.Now.ToString() + "\n";

        RollNumDice();
        RollStrDice();

        if (m_logContent.transform.childCount > 200)
        {
            Destroy(m_logContent.transform.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// add dice
    /// </summary>
    /// <param name="argDiceAmount">dice amount to make</param>
    /// <param name="argName">dice name</param>
    /// <param name="argType">dice type</param>
    /// <param name="argDice">dice main imfo</param>
    public void AddDice(int argDiceAmount, string argName, int argType, string[] argDice)
    {
        for (int i = 0; i < argDiceAmount; i++)
        {
            if (argType == 0 || argType == 1)
            {
                GameObject _groupObj = Instantiate(m_diceGroup);
                _groupObj.transform.SetParent(m_diceContent.transform);
                _groupObj.transform.localScale = new Vector3(1, 1, 1);

                DiceGroup _group = _groupObj.GetComponent<DiceGroup>();
                _group.m_mainDice.m_name = argName;
                _group.m_mainDice.m_type = argType;
                _group.m_mainDice.m_diceImfo = argDice;
                _group.m_mainDice.m_number = m_numDiceGroupList.Count;
                _group.m_mainDice.SetText();

                m_numDiceGroupList.Add(_group);
            }
            else
            {
                GameObject _diceObj = Instantiate(m_mainDice);
                _diceObj.transform.SetParent(m_noCalDiceContent.transform);
                _diceObj.transform.localScale = new Vector3(1, 1, 1);

                MainDice _dice = _diceObj.GetComponent<MainDice>();
                _dice.m_name = argName;
                _dice.m_type = argType;
                _dice.m_diceImfo = argDice;
                _dice.m_number = m_strDiceList.Count;
                _dice.SetText();

                m_strDiceList.Add(_dice);
            }
        }
    }

    /// <summary>
    /// remake dice
    /// </summary>
    /// <param name="argDiceNum">dice number(index)</param>
    /// <param name="argName">dice name</param>
    /// <param name="argType">dice type</param>
    /// <param name="argDice">dice main imfo</param>
    public void RemakeDice(int argDiceNum, string argName, int argType, int argNumChange, string[] argDice)
    {
        if (argType == 0 || argType == 1)
        {
            DiceGroup _diceGroup = m_numDiceGroupList[argDiceNum];

            _diceGroup.m_mainDice.m_name = argName;
            _diceGroup.m_mainDice.m_diceImfo = argDice;
            _diceGroup.m_mainDice.SetText();

            if (argDiceNum != argNumChange)
            {
                m_numDiceGroupList.RemoveAt(argDiceNum);
                m_numDiceGroupList.Insert(argNumChange , _diceGroup);
                m_numDiceGroupList[argNumChange].gameObject.transform.SetSiblingIndex(argNumChange);

                for (int i = m_numDiceGroupList.Count - 1; i >= 0; i--)
                {
                    m_numDiceGroupList[i].m_mainDice.m_number = i;
                    m_numDiceGroupList[i].gameObject.transform.SetAsFirstSibling();
                }
            }
        }
        else
        {
            MainDice _dice = m_strDiceList[argDiceNum];

            _dice.m_name = argName;
            _dice.m_diceImfo = argDice;
            _dice.SetText();

            if (argDiceNum != argNumChange)
            {
                m_strDiceList.RemoveAt(argDiceNum);
                m_strDiceList.Insert(argNumChange, _dice);
                m_strDiceList[argNumChange].gameObject.transform.SetSiblingIndex(argNumChange);
                
                for (int i = m_strDiceList.Count - 1; i >= 0; i--)
                {
                    m_strDiceList[i].m_number = i;
                    m_strDiceList[i].gameObject.transform.SetAsFirstSibling();
                }
            }
        }
    }

    /// <summary>
    /// setting operator type
    /// </summary>
    public void ChangeOperatorType(int argDiceGroupIndex, int argOpeType)
    {
        m_numDiceGroupList[argDiceGroupIndex].m_operator.ChangeOpeType(argDiceGroupIndex);
    }

    /// <summary>
    /// delete dice
    /// </summary>
    /// <param name="diceNum">dice num</param>
    public void DelDice(int argDiceNum, int argType)
    {
        if (argType == 0 || argType == 1)
        {
            Destroy(m_numDiceGroupList[argDiceNum].gameObject);
            m_numDiceGroupList.RemoveAt(argDiceNum);
        }
        else
        {
            Destroy(m_strDiceList[argDiceNum].gameObject);
            m_strDiceList.RemoveAt(argDiceNum);
        }
    }

    /// <summary>
    /// save dice group btn click
    /// </summary>
    public void ClickSaveGroup()
    {
        m_GroupNamePanel.SetActive(true);
    }

    /// <summary>
    /// save dice group btn click and save
    /// </summary>
    public void SaveDiceGroup()
    {
        if (m_groupNameInputField.text.Length < 1)
        {
            Alert("Name Is Too Short");
            return;
        }

        string[] files = Directory.GetFiles(
            Application.persistentDataPath + "/" + m_saveLoadManager.m_groupSaveFileName);
        foreach(string file in files)
        {
            if (Path.GetFileName(file) == m_groupNameInputField.text + ".json")
            {
                Alert("A group with the same name already exists");
                return;
            }
        }

        DiceGroupSaveData _save = new DiceGroupSaveData();
        _save.m_diceGroupName = m_groupNameInputField.text;

        for (int i = 0; i < m_numDiceGroupList.Count; i++)
        {
            _save.m_numDiceName.Add(m_numDiceGroupList[i].m_mainDice.m_name);
            _save.m_numDiceImfo.Add(m_numDiceGroupList[i].m_mainDice.m_diceImfo);

            _save.m_operatorType.Add(m_numDiceGroupList[i].m_operator.m_operatorType);
        }
        for (int i = 0; i < m_strDiceList.Count; i++)
        {
            _save.m_strDiceName.Add(m_strDiceList[i].m_name);
            _save.m_strDiceImfo.Add(m_strDiceList[i].m_diceImfo);
        }

        m_saveLoadManager.SaveGroupData(_save);
        m_GroupNamePanel.SetActive(false);
    }

    public void SetReAsk(ReAskDelegate argDel ,int arg0, int arg1)
    {
        m_reAsk = argDel;
        m_delArg0 = arg0;
        m_delArg1 = arg1;

        m_reAskPanel.SetActive(true);
    }

    /// <summary>
    /// click re ask ok button
    /// </summary>
    public void ClickReAsk(bool argAnswer)
    {
        if (argAnswer)
        {
            m_reAsk(m_delArg0, m_delArg1);
        }

        m_reAsk = null;
        m_reAskPanel.SetActive(false);
        m_delArg0 = 0;
        m_delArg1 = 0;
    }

    /// <summary>
    /// show loading panel
    /// </summary>
    /// <param name="argState">set state as bool</param>
    public void SetLoadingPanel(bool argState)
    {
        m_loadingPanel.SetActive(argState);
    }

    /// <summary>
    /// show alert
    /// </summary>
    public void Alert(string argStr)
    {
        m_alertPanel.SetActive(true);
        m_alertPanel.GetComponent<Animation>().Play();
        m_alertText.text = argStr;
    }

    /// <summary>
    /// instance
    /// </summary>
    public static DiceManager Instance
    {
        get
        {
            return g_diceManager;
        }
    }
}
