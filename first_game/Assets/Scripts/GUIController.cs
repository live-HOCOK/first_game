using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public GameObject losing_message;

    private void Start()
    {
        GameEvents.onBrickTouchingBottom.AddListener(OnBrickTouchingBottom);
    }

    private void OnBrickTouchingBottom()
    {
        losing_message.gameObject.SetActive(true);
    }

    public void OnClickLosingMessageBack()
    {
        losing_message.SetActive(false);
        GameEvents.onClickLoosingMessage.Invoke();
    }
}
