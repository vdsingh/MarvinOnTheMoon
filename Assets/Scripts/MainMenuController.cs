using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string levelOne;

    public void NewGameYes()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void loadTwo()
    {
        SceneManager.LoadScene("Aidan Level 1");
    }

    public void loadThree()
    {
        SceneManager.LoadScene("Aidan Level 2");
    }

    public void loadFour()
    {
        SceneManager.LoadScene("Vik's Level");
    }

    public void loadFive()
    {
        SceneManager.LoadScene("Ryan's Level");
    }
}
