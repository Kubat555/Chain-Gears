using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelInfo", menuName = "ChainGears/LevelInfo")]
public class LevelInfo : ScriptableObject {
    public List<Vector3> gearsPositions;
    public List<Vector3> gearsRotation;
    public List<Vector3> gearsScale;
    public List<int> gearsDirection;
}
