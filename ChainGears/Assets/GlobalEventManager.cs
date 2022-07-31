using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager : MonoBehaviour
{
    public static UnityEvent OnChainInGain = new UnityEvent();
    public static UnityEvent OnWinGame = new UnityEvent();
}
