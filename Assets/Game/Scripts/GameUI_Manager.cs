using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI_Manager : MonoBehaviour
{
    public GameManager GM;
    public TextMeshProUGUI coinText;
    public Slider healthSlider;
    public GameObject UI_Pause;
    public GameObject UI_GameOver;
    public GameObject UI_GameFinished;
    private enum GameUI_State { GamePlay, Pause, GameOver, GameFinished }
    GameUI_State currentState;

    private void Start()
    {
        SwitchUIState(GameUI_State.GamePlay);
        Debug.Log("Game Started!");
    }
    private void Update()
    {
        healthSlider.value = GM.playerCharacter.GetComponent<Health>().currentHealthPercentage;
        coinText.text = GM.playerCharacter.playerCoins.ToString();
    }

    private void SwitchUIState(GameUI_State state)
    {
        UI_Pause.SetActive(false);
        UI_GameOver.SetActive(false);
        UI_GameFinished.SetActive(false);

        Time.timeScale = 1;

        switch (state)
        {
            case GameUI_State.GamePlay:
                break;
            case GameUI_State.Pause:
                Time.timeScale = 0;
                UI_Pause.SetActive(true);
                break;
            case GameUI_State.GameOver:
                UI_GameOver.SetActive(true);
                break;
            case GameUI_State.GameFinished:
                UI_GameFinished.SetActive(true);
                break;
        }

        currentState = state;
    }
    public void TogglePauseUI()
    {
        if (currentState == GameUI_State.GamePlay)
        {
            SwitchUIState(GameUI_State.Pause);
        }
        else if (currentState == GameUI_State.Pause)
        {
            SwitchUIState(GameUI_State.GamePlay);
        }
    }

    public void Button_MainMenu()
    {

    }
    public void Button_Restart()
    {
        GM.Restart();
    }
}
