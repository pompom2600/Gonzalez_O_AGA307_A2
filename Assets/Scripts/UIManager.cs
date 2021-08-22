using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public PlayerMovement player;
    public TMP_Text lvlText;
    public Image[] hearts;
    public GameObject endScreenUI;

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }

        DeathCredit();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Loading menu");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void DeathCredit()
    {
        if (player.health <= 0)
        {
            endScreenUI.SetActive(true);
            Time.timeScale = 0f;
        }

    }
    public void UpdateHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i >= player.health)
                hearts[i].enabled = false;
            else
                hearts[i].enabled = true;
        }
    }

}
