using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain_test : MonoBehaviour
{
    ChainManager chainManager;
    private void Start()
    {
        chainManager = FindObjectOfType<ChainManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
            print("collision");
        if(collision.transform.tag == "Gear")
        {
            print("TRUE");
            GlobalEventManager.OnChainInGain.Invoke();
            chainManager._chainParent = transform;
            GetComponent<HingeJoint>().axis = new Vector3(0, 1, 0); 
            if(!ChainManager.chainParentList.Contains(this.transform))
            ChainManager.chainParentList.Add(this.transform);
        }
        if(collision.transform.tag == "BeginningOfChain")
        {
            ChainManager.isCollision = true;
        }
        if(gameObject.tag == collision.transform.tag )
        {
            print(collision.transform.tag);
            GameManager.isTwisted = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Gear")
        {
            GetComponent<HingeJoint>().axis = new Vector3(0, 0, 0);
        }
        if (collision.transform.tag == "BeginningOfChain")
        {
            ChainManager.isCollision = false;
        }
        if (gameObject.tag == collision.transform.tag)
        {
            GameManager.isTwisted = false;
        }
    }
}
