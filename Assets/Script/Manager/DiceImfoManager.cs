using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceImfoManager : MonoBehaviour
{
    /// <summary>
    /// glober DiceAddManager
    /// </summary>
    static DiceImfoManager g_diceInfoManager = null;

    /// <summary>
    /// save load manager
    /// </summary>
    public SaveLoadManager m_saveLoadManager = null;

    [Header("Info Panel")]
    /// <summary>
    /// dice imfo panel
    /// </summary>
    public GameObject m_diceImfoPanel = null;

    /// <summary>
    /// button group 0
    /// </summary>
    public GameObject m_buttonGroup0 = null;

    /// <summary>
    /// button group 1
    /// </summary>
    public GameObject m_buttonGroup1 = null;

    /// <summary>
    /// imfo panel main text
    /// </summary>
    public Text m_imfoPanelText = null;

    /// <summary>
    /// dice type dropdown
    /// </summary>
    public Dropdown m_diceTypeDropDown = null;

    /// <summary>
    /// dice name input field
    /// </summary>
    public InputField m_diceNameInputField = null;

    /// <summary>
    /// dice sides input field
    /// </summary>
    public InputField m_diceSidesInputField = null;

    /// <summary>
    /// dice amount to add input field
    /// </summary>
    public InputField m_diceAmountInputField = null;

    /// <summary>
    /// oreder to calculate input field
    /// </summary>
    public InputField m_CalOrderInputField = null;

    [Header("Dice Sample")]
    /// <summary>
    /// dice sample scroll view
    /// </summary>
    public GameObject m_diceSampleScrollView = null;

    /// <summary>
    /// content input dice sample
    /// </summary>
    public GameObject m_addDiceSampleContent = null;

    /// <summary>
    /// dice sample img prefeb
    /// </summary>
    public Image m_addDiceSampleImg = null;

    /// <summary>
    /// sample dice
    /// </summary>
    public string[] m_sampledice = null;

    /// <summary>
    /// type at before
    /// </summary>
    int m_beforeType = 0;

    /// <summary>
    /// now mode
    /// 0 = add dice, 1 = reforme dice
    /// </summary>
    int m_nowMode = 0;

    /// <summary>
    /// now remake dice number(index)
    /// </summary>
    int m_nowRemakeDice = 0;

    // Start is called before the first frame update
    void Start()
    {
        g_diceInfoManager = this;
    }

    /// <summary>
    /// open add mode
    /// </summary>
    public void OpenAddDicePanel()
    {
        m_diceImfoPanel.SetActive(true);
        m_buttonGroup0.SetActive(true);
        m_buttonGroup1.SetActive(false);
        m_nowMode = 0;

        m_imfoPanelText.text = "Add Dice";
        m_diceNameInputField.text = "Dice";
        m_diceAmountInputField.text = "1";
        m_diceSidesInputField.text = "6";

        RefreshSample();
    }

    /// <summary>
    /// Get dice Imfo and ready to reforme
    /// </summary>
    public void GetDiceImfo(MainDice argDice)
    {
        m_diceImfoPanel.SetActive(true);
        m_buttonGroup0.SetActive(false);
        m_buttonGroup1.SetActive(true);
        m_nowMode = 1;
        m_nowRemakeDice = argDice.m_number;

        m_imfoPanelText.text = "Dice Imformation";
        m_diceSidesInputField.text = argDice.m_diceImfo.Length.ToString();
        m_sampledice = argDice.m_diceImfo;

        m_diceNameInputField.text = argDice.m_name;
        m_CalOrderInputField.text = argDice.m_number.ToString();

        RefreshSample();
    }

    /// <summary>
    /// refresh sample dice
    /// </summary>
    public void RefreshSample()
    {
        if(m_diceSidesInputField.text.Length <= 0)
        {
            return;
        }

        int _diceSides = int.Parse(m_diceSidesInputField.text);
        if (_diceSides + 1 <= 0)
        {
            return;
        }

        for (int i = 0; i < m_addDiceSampleContent.transform.childCount; i++)
        {
            Destroy(m_addDiceSampleContent.transform.GetChild(i).gameObject);
        }

        string[] _save = null;
        if(m_beforeType != 0)
        {
            _save = m_sampledice;
        }
        m_sampledice = null;
        m_sampledice = new string[_diceSides];

        if (m_diceTypeDropDown.value == 0)
        {
            for (int i = 0; i < _diceSides; i++)
            {
                m_sampledice[i] = (i + 1).ToString();

                Image _sampleImg = Instantiate(m_addDiceSampleImg);
                _sampleImg.transform.SetParent(m_addDiceSampleContent.transform);
                _sampleImg.transform.localScale = new Vector3(1, 1, 1);

                DiceSampleImg _sampleScript = _sampleImg.GetComponent<DiceSampleImg>();
                _sampleScript.m_inputfield.text = (i + 1).ToString();
                _sampleScript.m_inputfield.interactable = false;
                _sampleScript.m_number = i;
            }

            m_beforeType = 0;
        }
        else if (m_diceTypeDropDown.value == 1)
        {
            for (int i = 0; i < _diceSides; i++)
            {
                Image _sampleImg = Instantiate(m_addDiceSampleImg);
                _sampleImg.transform.SetParent(m_addDiceSampleContent.transform);
                _sampleImg.transform.localScale = new Vector3(1, 1, 1);
                
                DiceSampleImg _sampleScript = _sampleImg.GetComponent<DiceSampleImg>();
                _sampleScript.m_inputfield.interactable = true;
                _sampleScript.m_inputfield.contentType = InputField.ContentType.DecimalNumber;
                _sampleScript.m_number = i;

                if (_save != null)
                {
                    if (i < _save.Length)
                    {
                        m_sampledice[i] = _save[i];
                        _sampleImg.GetComponent<DiceSampleImg>().m_inputfield.text = _save[i];
                    }
                }
            }

            m_beforeType = 1;
        }
        else if (m_diceTypeDropDown.value == 2)
        {
            for (int i = 0; i < _diceSides; i++)
            {
                Image _sampleImg = Instantiate(m_addDiceSampleImg);
                _sampleImg.transform.SetParent(m_addDiceSampleContent.transform);
                _sampleImg.transform.localScale = new Vector3(1, 1, 1);

                DiceSampleImg _sampleScript = _sampleImg.GetComponent<DiceSampleImg>();
                _sampleScript.m_inputfield.interactable = true;
                _sampleScript.m_number = i;

                if (_save != null)
                {
                    if (i < _save.Length)
                    {
                        m_sampledice[i] = _save[i];
                        _sampleImg.GetComponent<DiceSampleImg>().m_inputfield.text = _save[i];
                    }
                }
            }

            m_beforeType = 2;
        }
        else if (m_diceTypeDropDown.value == 3)
        {
            for (int i = 0; i < _diceSides; i++)
            {
                Image _sampleImg = Instantiate(m_addDiceSampleImg);
                _sampleImg.transform.SetParent(m_addDiceSampleContent.transform);
                _sampleImg.transform.localScale = new Vector3(1, 1, 1);

                DiceSampleImg _sampleScript = _sampleImg.GetComponent<DiceSampleImg>();
                _sampleScript.m_inputfield.placeholder.GetComponent<Text>().text = "";
                _sampleScript.m_inputfield.interactable = false;
                _sampleScript.m_number = i;
            }

            m_beforeType = 3;
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// remake dice
    /// </summary>
    public void RemakeDice()
    {
        if(m_nowMode != 1)
        {
            m_diceImfoPanel.SetActive(false);
            return;
        }

        if (m_diceAmountInputField.text.Length <= 0 ||
            m_diceNameInputField.text.Length <= 0 ||
            m_CalOrderInputField.text.Length <= 0 ||
            m_sampledice.Length <= 0)
        {
            DiceManager.Instance.Alert("Not Allow Empty Space");
            return;
        }

        DiceManager.Instance.RemakeDice(m_nowRemakeDice, m_diceNameInputField.text, m_diceTypeDropDown.value,
            int.Parse(m_CalOrderInputField.text), m_sampledice);
        m_diceImfoPanel.SetActive(false);
    }

    public void DelDice()
    {
        if (m_nowMode != 1)
        {
            m_diceImfoPanel.SetActive(false);
            return;
        }

        DiceManager.Instance.DelDice(m_nowRemakeDice, m_diceTypeDropDown.value);

        m_diceImfoPanel.SetActive(false);
    }

    /// <summary>
    /// add dice
    /// </summary>
    public void AddDice()
    {
        if (!CheckMode0()) return;

        DiceManager.Instance.AddDice(int.Parse(m_diceAmountInputField.text), m_diceNameInputField.text,
            m_diceTypeDropDown.value, m_sampledice);
    }

    /// <summary>
    /// save dice imfo
    /// </summary>
    public void SaveDice()
    {
        if (!CheckMode0()) return;

        DiceData _dice = new DiceData();
        _dice.m_name = m_diceNameInputField.text;
        _dice.m_type = m_diceTypeDropDown.value;
        _dice.m_diceImfo = m_sampledice;

        if (m_diceTypeDropDown.value == 1 || m_diceTypeDropDown.value == 0)
        {
            for (int i = 0; i < m_saveLoadManager.m_saveNumDiceList.Count; i++)
            {
                if(m_saveLoadManager.m_saveNumDiceList[i].m_name == m_diceNameInputField.text)
                {
                    DiceManager.Instance.Alert("A dice with the same name already exists");
                    return;
                }
            }

            m_saveLoadManager.m_saveNumDiceList.Add(_dice);
        }
        else
        {
            for (int i = 0; i < m_saveLoadManager.m_saveStrDiceList.Count; i++)
            {
                if (m_saveLoadManager.m_saveStrDiceList[i].m_name == m_diceNameInputField.text)
                {
                    DiceManager.Instance.Alert("A dice with the same name already exists");
                    return;
                }
            }

            m_saveLoadManager.m_saveStrDiceList.Add(_dice);
        }
        m_saveLoadManager.SaveData();

        DiceManager.Instance.Alert("Save Complete");
    }

    bool CheckMode0()
    {
        if (m_nowMode != 0)
        {
            m_diceImfoPanel.SetActive(false);
            return false;
        }

        if (m_diceAmountInputField.text.Length <= 0 ||
            m_diceNameInputField.text.Length <= 0 ||
            m_sampledice.Length <= 0)
        {
            DiceManager.Instance.Alert("Not Allow Empty Space");
            return false;
        }

        return true;
    }

    /// <summary>
    /// instance
    /// </summary>
    public static DiceImfoManager Instance
    {
        get
        {
            return g_diceInfoManager;
        }
    }
}
