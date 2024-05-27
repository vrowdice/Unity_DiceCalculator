using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyBtn : MonoBehaviour
{
    public void Click(Text argText)
    {
        GUIUtility.systemCopyBuffer = argText.text;
        DiceManager.Instance.Alert("Log Has Been Copied");
    }
}
