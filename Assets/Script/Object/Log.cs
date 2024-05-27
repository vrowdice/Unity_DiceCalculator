using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Log : MonoBehaviour, IPointerClickHandler
{
    float clickTime = 0;

    void OnMouseDoubleClick()
    {
        GUIUtility.systemCopyBuffer = GetComponent<Text>().text;
        DiceManager.Instance.Alert("Log Has Been Copied");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time - clickTime) < 0.3f)
        {
            OnMouseDoubleClick();
            clickTime = -1;
        }
        else
        {
            clickTime = Time.time;
        }
    }
}
