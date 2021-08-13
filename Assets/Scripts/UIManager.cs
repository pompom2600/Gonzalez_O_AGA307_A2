using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public PlayerMovement player;


    public TMP_Text lvlText;
    public Slider torchSlider;
    public Image[] hearts;


    private void Start()
    {
        if (instance == null)
            instance = this;
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





    public void UpdateScore(int _score)
    {
        lvlText.text = "Score: " + _score;
    }



    public void UpdateTimer(float _timer)
    {
       // lvlText.text = "Timer: " + _timer.ToString("F2");
       // torchSlider.value = _timer;

       // lvlText.color = _timer < 10f ? Color.red : Color.white;
        /*if (_timer < 10f)
            timerText.color = Color.red;
        else
            timerText.color = Color.white;
        */
    }


}
