using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainController : MonoBehaviour
{
    
    // [SerializeField] GameObject chain;
    // [SerializeField] bool isHead = false;
    // static ChainController activeChain;

    
    private void Start() {
        
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Chain.instance.radiusForSpawn);
    }
}
