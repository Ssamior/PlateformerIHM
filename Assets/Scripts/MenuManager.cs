using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void RulesMenu()
    {
        SceneManager.LoadScene("Rules");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
