using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainController : MonoBehaviour
{
    
    // [SerializeField] GameObject chain;
    // [SerializeField] bool isHead = false;
    // static ChainController activeChain;
    public float radiusForSpawn;

    [HideInInspector]
    public Rigidbody rb;
    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusForSpawn);
    }
}
