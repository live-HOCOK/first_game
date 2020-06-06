using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{

    public static UnityEvent onDestroyAllBalls = new UnityEvent();
    public static UnityEvent onBrickTouchingBottom = new UnityEvent();
    public static UnityEvent onClickLoosingMessage = new UnityEvent();

}
