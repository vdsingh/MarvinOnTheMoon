using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject player;
    public TMP_Text gameOverText;
    public GameObject nextLevelButton;

    public string nextLevel;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<FPS_Player>().gameOver)
        {
            gameOverPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if(GameObject.Find("HUD") != null)
                GameObject.Find("HUD").SetActive(false);
            if(player.GetComponent<FPS_Player>().health <= 0)
            {
                gameOverText.text = "You Died";
                nextLevelButton.SetActive(false);
            }
            else
            {
                gameOverText.text = "You Win";
                nextLevelButton.SetActive(true);
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
