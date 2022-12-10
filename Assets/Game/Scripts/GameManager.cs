using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameUI_Manager gameUI_Manager;
    public Character playerCharacter;
    private bool gameIsOver;

    private void Awake()
    {
        playerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER!");
    }

    public void GameFinished()
    {
        Debug.Log("GAME IS FINISHED! YOU WIN!");
    }

    private void Update()
    {
        if (gameIsOver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            gameUI_Manager.TogglePauseUI();

        if (playerCharacter.currentState == Character.CharacterState.Dead)
        {
            gameIsOver = true;
            GameOver();
        }
    }

    public void ReturnToTheMainMenu()
    {

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
