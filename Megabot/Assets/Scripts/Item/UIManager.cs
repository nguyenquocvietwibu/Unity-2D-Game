using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseUI;
    private bool isPaused = false;

    public void Update()
    {
        if(SceneManager.GetActiveScene().name != ("Start"))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f; // Dừng thời gian
        isPaused = true;
    }

    void ResumeGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục thời gian
        isPaused = false;
    }
    public void OnRestartPress()
     {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
     }
     public void OneGameResumePress()
     {
        ResumeGame();
     }
     public void OnGameExitPress()
     {
        SceneManager.LoadScene("Start");
     }
     public void OnStartGamePress()
     {
        SceneManager.LoadScene("Level 1");
     }
}
