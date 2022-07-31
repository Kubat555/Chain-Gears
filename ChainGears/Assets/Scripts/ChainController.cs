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
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
