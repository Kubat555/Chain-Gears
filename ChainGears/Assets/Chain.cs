using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField] List<ChainController> chainParts;
    public List<ChainController> chains {get{return chainParts;} private set{}}
    public static Chain instance;
    public float radiusForSpawn;

    Ray ray;
    int index = 0;
    bool firstChainPart;

    private void Awake() {
        if(instance == null){
            instance = this;
        }
        index = 0;
        firstChainPart = true;
    }

    private void Update() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    void FixedUpdate()
    {
        if(Input.GetMouseButton(0) && firstChainPart){
            if(Physics.Raycast(ray, out RaycastHit raycastHit)){
                chainParts[index].transform.position = new Vector3(raycastHit.point.x, 0.6f, raycastHit.point.z);
            }
            firstChainPart = false;
        }
        else if(Input.GetMouseButton(0)){
            
            if(Physics.Raycast(ray, out RaycastHit raycastHit)){
                // chainParts[index].transform.position = raycastHit.point;

                if(Mathf.Abs(chainParts[index].transform.position.z - raycastHit.point.z) > radiusForSpawn || Mathf.Abs(chainParts[index].transform.position.x - raycastHit.point.x) > radiusForSpawn){
                    index = Mathf.Clamp(index + 1, 0, chainParts.Count - 1);
                    chainParts[index].transform.position = new Vector3(raycastHit.point.x, 0.6f, raycastHit.point.z);
                }
            }
            
        }
    }

}
