using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string levelOne;
    private string selectedLevel;

    public void NewGameYes()
    {
        SceneManager.LoadScene(levelOne);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(selectedLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
