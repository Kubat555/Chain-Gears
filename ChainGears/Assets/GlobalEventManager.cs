using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager : MonoBehaviour
{
    public static UnityEvent OnChainInGain = new UnityEvent();
    public static UnityEvent OnEndDrawing = new UnityEvent();
    public static UnityEvent OnWinGame = new UnityEvent();
    public static UnityEvent OnLoseGame = new UnityEvent();
    public static UnityEvent RotateStart = new UnityEvent();
    public static UnityEvent RotateStop = new UnityEvent();


}
