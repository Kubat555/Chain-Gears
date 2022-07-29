using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Node")){
            transform.position = other.transform.position;
        }
    }
}
