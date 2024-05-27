using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenManager : MonoBehaviour
{
    public string m_scenName = "";

    /// <summary>
    /// go main scene
    /// </summary>
    public void Click(){
        SceneManager.LoadScene(m_scenName);
    }
}
