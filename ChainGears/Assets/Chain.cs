using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField] List<ChainController> chainParts;
    public List<ChainController> chains {get{return chainParts;} private set{}}
    public static Chain instance;

    Ray ray;
    int index = 0;
    bool firstChainPart;
    bool canConnection;
    bool chainIsDone;

    private void Awake() {
        if(instance == null){
            instance = this;
        }
        index = 0;
        firstChainPart = true;
        canConnection = false;
        chainIsDone = false;
    }



    void FixedUpdate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Input.GetMouseButton(0) && firstChainPart){
            if(Physics.Raycast(ray, out RaycastHit raycastHit)){
                chainParts[index].transform.position = new Vector3(raycastHit.point.x, 0.6f, raycastHit.point.z);
            }
            firstChainPart = false;
        }
        else if(Input.GetMouseButton(0) && !chainIsDone){
            
            if(Physics.Raycast(ray, out RaycastHit raycastHit)){
                // chainParts[index].transform.LookAt(raycastHit.point, Vector3.right);
                if(canConnection){
                    if((Mathf.Abs(chainParts[index].transform.position.z - raycastHit.point.z) > chainParts[index].radiusForSpawn || Mathf.Abs(chainParts[index].transform.position.x - raycastHit.point.x) > chainParts[index].radiusForSpawn) && (Mathf.Abs(chainParts[0].transform.position.z - raycastHit.point.z) < chainParts[0].radiusForSpawn || Mathf.Abs(chainParts[0].transform.position.x - raycastHit.point.x) < chainParts[0].radiusForSpawn)){

                        index = Mathf.Clamp(index + 1, 0, chainParts.Count - 1);
                        chainParts[index].transform.position = new Vector3(raycastHit.point.x, 0.6f, raycastHit.point.z);

                        HingeJoint joint = chainParts[0].gameObject.AddComponent<HingeJoint>();
                        joint.connectedBody =  chainParts[index].rb;
                        joint.useSpring = true;
                        JointSpring hingeSpring = joint.spring;
                        hingeSpring.spring = 30;
                        hingeSpring.damper = 10;
                        joint.spring = hingeSpring;
                        canConnection = true;
                        chainIsDone = true;
                    }
                }


                else if(Mathf.Abs(chainParts[index].transform.position.z - raycastHit.point.z) > chainParts[index].radiusForSpawn || Mathf.Abs(chainParts[index].transform.position.x - raycastHit.point.x) > chainParts[index].radiusForSpawn){
                    index = Mathf.Clamp(index + 1, 0, chainParts.Count - 1);
                    chainParts[index].transform.position = new Vector3(raycastHit.point.x, 0.6f, raycastHit.point.z);
                    ConfigureJointChainPart();
                }
            }
            
        }
    }

    void ConfigureJointChainPart(){
        HingeJoint joint = chainParts[index].gameObject.AddComponent<HingeJoint>();
        joint.connectedBody =  chainParts[index - 1].rb;
        joint.useSpring = true;
        JointSpring hingeSpring = joint.spring;
        hingeSpring.spring = 30;
        hingeSpring.damper = 10;
        joint.spring = hingeSpring;
        canConnection = true;
    }

}
