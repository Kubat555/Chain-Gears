using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain_test : MonoBehaviour
{
    ChainManager chainManager;
    private void Start()
    {
        chainManager = FindObjectOfType<ChainManager>();
        GlobalEventManager.OnChainBreaks.AddListener(BreakChain);
    }
    private void OnCollisionEnter(Collision collision)
    { 
        if(collision.transform.tag == "Gear")
        { 
            chainManager._chainParent = transform;
            GetComponent<HingeJoint>().axis = new Vector3(0, 1, 0); 
            if(!ChainManager.chainParentList.Contains(this.transform))
            ChainManager.chainParentList.Add(this.transform);
        }
        if(collision.transform.tag == "BeginningOfChain")
        {
            ChainManager.isCollision = true;
        }
        if(collision.transform.tag == "RightWay" && !chainManager.gearList.Contains(collision.gameObject))
        {
            chainManager.gearList.Add(collision.gameObject);
        }
        if(gameObject.tag == collision.transform.tag && !ChainManager.twistedChainList.Contains(collision.gameObject) )
        {
            ChainManager.twistedChainList.Add(collision.gameObject); 
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
        if (collision.transform.tag == "RightWay" /*&& chainManager.gearList.Contains(collision.gameObject)*/)
        {
            chainManager.gearList.Remove(collision.gameObject);
        }
        if (gameObject.tag == collision.transform.tag &&  ChainManager.twistedChainList.Contains(collision.gameObject))
        {
            ChainManager.twistedChainList.Remove(collision.gameObject);
            if (ChainManager.twistedChainList.Count <= 0)
                GameManager.isTwisted = false;
        }
    }

    private void BreakChain()
    {
        transform.parent = null;
        StartCoroutine(DestroyChain());

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
       // GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
       Destroy(GetComponent<HingeJoint>());
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10,10), 0, Random.Range(-10, 10)), ForceMode.Impulse);
        GetComponent<BoxCollider>().enabled = false;
       GameManager.isTwisted = false;
        ChainManager.isCollision = false;
       chainManager.gearList.Clear();
    }

    IEnumerator DestroyChain()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
