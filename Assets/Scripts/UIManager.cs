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
        if (Input.GetKeyDown(KeyCode.Escape)) // Pause things
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

    public void Resume() //Resume button
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause() //Pause the game
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu() //Load Menu
    {
        Time.timeScale = 1f;
        Debug.Log("Loading menu");
    }
    
    public void QuitGame() //Quit Game
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void DeathCredit() //Death Credit update
    {
        if (player.health <= 0)
        {
            endScreenUI.SetActive(true);
            Time.timeScale = 0f;
        }

    }
    public void UpdateHealth() //Heart Image update
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
