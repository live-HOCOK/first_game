using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public GameObject losingMessage;
    public GameObject mainMenu;
    public GameObject levelMenu;

    private void Start()
    {
        GameEvents.onBrickTouchingBottom.AddListener(OnBrickTouchingBottom);
    }

    private void OnBrickTouchingBottom()
    {
        losingMessage.gameObject.SetActive(true);
    }

    public void OnClickLosingMessageBack()
    {
        losingMessage.SetActive(false);
        GameEvents.onClickLoosingMessage.Invoke();
    }

    public void OnClickStart()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(true);
    }

    public void OnClickLevel1()
    {
        levelMenu.SetActive(false);
        GetComponent<GameplayController>().SetLevel(1);
        GameEvents.onStartGame.Invoke();
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
