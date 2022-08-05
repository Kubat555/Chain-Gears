using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightCollideCheck : MonoBehaviour
{
      private void OnCollisionStay(Collision other) {
        if(other.gameObject.CompareTag("Chain")&&!ChainManager.Instance.gearList.Contains(gameObject)){
            ChainManager.Instance.gearList.Add(gameObject);
        }
    }
    private void OnCollisionExit(Collision other) {
        if(other.gameObject.CompareTag("Chain") && ChainManager.Instance.gearList.Contains(gameObject)){
            ChainManager.Instance.gearList.Remove(gameObject);
        }
    }
    
}
