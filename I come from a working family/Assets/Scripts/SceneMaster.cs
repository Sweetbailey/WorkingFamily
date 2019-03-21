using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void EndGame()
    {
        Application.Quit();
    }
    public void Controls()
    {
        SceneManager.LoadScene("ControlScene");
    }
}
