using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject goGameplayController;
    public GameObject goPlayerControl;

    private GameplayController gameplayController;
    private PlayerCotrol playerCotrol;

    private void Start()
    {
        gameplayController = goGameplayController.GetComponent<GameplayController>();
        playerCotrol = goPlayerControl.GetComponent<PlayerCotrol>();
        GameEvents.onStartGame.AddListener(onStartGame);
    }

    private void onStartGame()
    {
        gameplayController.enabled = true;
        playerCotrol.enabled = true;
    }

}
