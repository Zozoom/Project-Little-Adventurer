using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Character playerCharacter;
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

        if (playerCharacter.currentState == Character.CharacterState.Dead)
        {
            gameIsOver = true;
            GameOver();
        }
    }
}
