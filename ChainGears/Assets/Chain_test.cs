using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain_test : MonoBehaviour
{
    public bool loseCollide;
    ChainManager chainManager;
    private void Start()
    {
        loseCollide = false;
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
        if(chainManager.chainsList[chainManager.chainsList.Count-1].tag == collision.transform.tag && !ChainManager.twistedChainList.Contains(chainManager.chainsList[chainManager.chainsList.Count-1]) )
        {
            ChainManager.twistedChainList.Add(chainManager.chainsList[chainManager.chainsList.Count-1]); 
            //GameManager.isTwisted = true; 
        }
        if(collision.transform.tag == "GameOver"){
            ChainManager.gameOver = true;
            ChainManager.gameOverCollideCount++;
            loseCollide = true;
            Debug.Log(ChainManager.gameOverCollideCount);
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
        if (gameObject.tag == collision.transform.tag )
        {
            ChainManager.twistedChainList.Remove(collision.gameObject);
           // if (ChainManager.twistedChainList.Count <= 0)
            //    GameManager.isTwisted = false;
        }
        if(collision.transform.tag == "GameOver" && ChainManager.gameOverCollideCount > 0){
            ChainManager.gameOverCollideCount = Mathf.Clamp(ChainManager.gameOverCollideCount - 1, 0, 100);
            Debug.Log(ChainManager.gameOverCollideCount);
        }
        if(collision.transform.tag == "GameOver" && ChainManager.gameOverCollideCount == 0){
            ChainManager.gameOver = false;
            loseCollide = false;
            Debug.Log("Игрок уже не проиграет");

        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("BeginningOfChain") && ChainManager.Instance.chainsList.Count > 5&&!ChainManager.isCollision){
           // chainManager._chainParent = chainManager._currentParent;
            Debug.Log("before remove" + chainManager.chainsList.Count);
            int currentIndex = ChainManager.Instance.chainsList.IndexOf(gameObject);

            if(currentIndex != ChainManager.Instance.chainsList.Count - 1){
                GameObject gb =  chainManager.chainsList[currentIndex + 1];
                chainManager.chainsList.RemoveRange(currentIndex + 1, ChainManager.Instance.chainsList.Count - (currentIndex+ 1));
                Destroy(gb);
                Debug.Log("after remove" + chainManager.chainsList.Count);
            }
            
            chainManager._chainParent.LookAt(other.transform);
            other.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
            ChainManager.isCollision = true;
            
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
        GetComponent<SphereCollider>().enabled = false;
      // GameManager.isTwisted = false;
        ChainManager.isCollision = false;
       chainManager.gearList.Clear();
        GameManager.Instance.rotateButton.interactable = false;
    }

    IEnumerator DestroyChain()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
