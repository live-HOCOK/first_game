using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGUIController : MonoBehaviour
{

    public void OnClickStart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

}
