using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{

    public static UnityEvent onDestroyAllBalls = new UnityEvent();
    public static UnityEvent onStartShooting = new UnityEvent();
    public static UnityEvent onStopShooting = new UnityEvent();

}
