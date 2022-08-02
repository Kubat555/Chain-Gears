using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class ChainWithSpline : MonoBehaviour
{
    [SerializeField] SplineComputer spline;
    [SerializeField] List<SplineFollower> chains;
    double chainPosition = 0;
    int index = 0;
    private void Start() {
        chainPosition = 0;
        index = 0;
    }

    public void chainFollow(){
        while(chainPosition < 1){
            chains[index].spline = spline;
            chains[index].EvaluatePosition(chainPosition);
            chainPosition =  (spline.CalculateLength() / chains.Count) + chainPosition;
            index++;
        }
        index = 0;
        chainPosition = 0;
    }
}
